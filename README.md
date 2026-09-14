[README (1).md](https://github.com/user-attachments/files/32175586/README.1.md)
# QarzAlHasana — Interest-Free Loan Fund API

A backend for a *qarz al-hasana* fund: a community savings pool where members pay
dues, guarantee each other's loans, borrow without interest, and repay in
instalments. Built with .NET 10, Clean Architecture, CQRS, and EF Core.

The domain is small enough to read in one sitting and real enough that getting it
wrong costs someone money. That combination is why this project exists — the
interesting part is not the CRUD, it's the rules underneath it and where they are
enforced.

---

## Contents

- [Domain in one minute](#domain-in-one-minute)
- [Architecture](#architecture)
- [Design decisions](#design-decisions)
- [Business rules](#business-rules)
- [Security model](#security-model)
- [Testing](#testing)
- [API surface](#api-surface)
- [Running it locally](#running-it-locally)
- [Known gaps](#known-gaps)

---

## Domain in one minute

A *qarz al-hasana* fund lends money without interest. Members pay a registration
fee and monthly dues into a shared pool; when someone needs a loan, other members
guarantee it and the fund lends from the pool. Late repayment carries a penalty,
capped — the goal is to discourage delay, not to profit from it.

The lifecycle the API implements:

```
register → admin approves identity → pay registration fee → admin confirms fee
   → request a loan → guarantors confirm → admin approves
   → instalments generated → repay in order, penalty if late
```

Each arrow is a rule, and each rule lives in exactly one place. Which place, and
why, is the substance of this README.

---

## Architecture

Four projects, dependencies pointing inward:

| Project | Contents | Depends on |
|---|---|---|
| `Domain` | Entities, enums, business rules, `BusinessRuleException` | nothing |
| `Application` | Commands, queries, handlers, validators, repository interfaces | Domain |
| `Infrastructure` | EF Core, repositories, JWT generation, BCrypt hashing | Application |
| `Api` | Controllers, contracts, exception handlers, `CurrentUserService` | Application, Infrastructure |
| `Domain.Tests` | xUnit tests over domain rules | Domain |

`Domain` references nothing at all — not EF Core, not MediatR. It does not know a
database exists.

**MediatR** for dispatch, with pipeline behaviours for logging and validation.
**FluentValidation** for input shape. **BCrypt** (work factor 12) for passwords.
**SQL Server** via EF Core.

There is no generic `IRepository<T>`. Each repository exposes exactly the methods
its feature needs, which keeps read paths from dragging entities around when they
only need seven columns.

Exceptions map to status codes through `IExceptionHandler` implementations:

| Exception | Status | Meaning |
|---|---|---|
| `ValidationException` | 400 | The request is malformed |
| *(no token)* | 401 | Not authenticated |
| `ForbiddenException` | 403 | Authenticated, but not yours |
| `NotFoundException` | 404 | No such thing |
| `BusinessRuleException` | 409 | Well-formed, but the domain says no |

`BusinessRuleException` carries a machine-readable code (`PAYMENT_AMOUNT_MISMATCH`,
`INSTALLMENT_OUT_OF_ORDER`) alongside the human message, so a client can branch on
the rule rather than parse prose.

---

## Design decisions

### Store the fact, compute the opinion

`InstallmentStatus` has two values: `Unpaid` and `Paid`. There is no `Overdue`.

Whether an instalment is late follows from `DueDate` and `Status`, so it is
computed — `IsOverdue(asOf)` — never stored. A stored `Overdue` flag would need a
nightly job to stay truthful and would be wrong between runs.

Penalties go the other way. `PenaltyAmount` and `PaidAmount` are frozen onto the
instalment at the moment of payment:

```csharp
Status = InstallmentStatus.Paid;
PaidDate = paymentDate;
PenaltyAmount = penalty;
PaidAmount = totalDue;
```

Once money has changed hands, the figure is a fact that happened, not a view of
the present. Recomputing it later would let a rule change rewrite history — a
member who paid 30 in penalties last year should not owe 50 because the rate
moved.

**The rule: store what happened, compute what is currently true.**

### The compiler guards the aggregate root

`Installment.Pay(...)` is `internal`. `Guarantor.Confirm(...)` is `internal`. The
Application layer is a separate assembly, so it cannot call either. The only
doors in are `LoanRequest.PayInstallment(...)` and
`LoanRequest.ConfirmGuarantor(...)`.

This is not stylistic. Two rules live on the aggregate root and would be
trivially bypassed by reaching for the child directly:

- instalments must be paid oldest-first
- the amount must equal instalment + penalty exactly — no partial payment, no
  paying the penalty separately

And on the guarantee side, only the guarantor themselves may confirm their own
commitment. Routing that through the root is what makes it checkable in one
place.

A rule the compiler enforces cannot be forgotten during review. The test project
is a separate assembly too, which means even the tests must go through the
aggregate root — so they exercise the real path rather than a shortcut.

### Time is a parameter, not a call

No entity reads `DateTime.UtcNow` inside its penalty logic. `asOf` is passed in
from the caller. That single choice is what makes the financial core testable
without a database, a clock, or a mock:

```csharp
installment.CalculatePenalty(dueDate.AddDays(5));    // 0    — grace period
installment.CalculatePenalty(dueDate.AddDays(6));    // 30   — first block
installment.CalculatePenalty(dueDate.AddDays(11));   // 30   — block not complete
installment.CalculatePenalty(dueDate.AddDays(12));   // 60   — second block
installment.CalculatePenalty(dueDate.AddDays(1000)); // 480  — capped
```

Those five lines are five unit tests. With `UtcNow` buried in the entity,
expressing "assume six days have passed" would mean changing the system clock.

### CQRS, and what it buys

Write paths load entities and enforce rules. Read paths project straight to DTOs
inside the repository, with `AsNoTracking`, so EF emits a `SELECT` over only the
needed columns and no `Include` is required:

```csharp
return await _context.LoanRequests
    .AsNoTracking()
    .Where(lr => lr.Status == LoanStatus.Pending)
    .OrderBy(lr => lr.RequestDate)
    .Select(lr => new PendingLoanRequestDto { /* seven columns */ })
    .ToListAsync(cancellationToken);
```

DTOs are shaped per audience, not per entity. A member's payment history carries
the rejection reason so they know what to fix; the admin's pending list does not.
The admin's ticket list carries the member's name; the member's own list does
not. A shared DTO would have to either leak fields or withhold them.

Where a value would need C# logic EF cannot translate — `TotalDue(asOf)` — the
handler loads the entity and computes in memory rather than contorting the query.
And that logic lives in one query only; the loan detail endpoint deliberately
omits instalments rather than duplicating the calculation.

### Missing configuration is an error, not a default

`GetRegistrationFeeAsync` throws when `FundSettings` is empty. It used to return
`0`. Nothing crashed — every membership payment was simply recorded as free.

That is the same failure mode as a missing `Include`: a loan repository read
returned an empty `Installments` collection, so `All(i => i.Status == Paid)` was
vacuously true and the loan was marked fully repaid without a single payment. No
exception. A confident, wrong answer.

Both bugs are fixed, and both are the reason this codebase prefers throwing over
defaulting. **A silently wrong number is worse than a crash** — the crash gets
noticed.

### Two gates, not one

`IsActive` and `HasPaidRegistrationFee` look similar and are deliberately kept
apart.

`IsActive` answers *are you who you say you are* — an admin verifies the national
code and phone number before the account can log in.
`HasPaidRegistrationFee` answers *are you financially a member* — it gates loan
requests and nothing else.

Collapsing them deadlocks: a member cannot record a payment without logging in,
and cannot log in without having paid.

---

## Business rules

Where each rule is enforced, and what breaks if you move it.

| Rule | Lives in |
|---|---|
| Penalty: 3% per completed 6-day block, capped at 48% | `Installment.CalculatePenalty` |
| Payment must equal instalment + penalty exactly | `Installment.Pay` |
| Instalments are paid oldest-first | `LoanRequest.PayInstallment` |
| A loan needs at least one guarantor, all confirmed, before approval | `LoanRequest.Approve` |
| Nobody guarantees their own loan | `LoanRequest.AddGuarantor` |
| Total guaranteed cannot exceed the loan | `LoanRequest.AddGuarantor` |
| Only the guarantor confirms their own guarantee | `LoanRequest.ConfirmGuarantor` |
| Monthly dues carry a period; registration fees do not | `MembershipPayment.Create` |
| Only a pending payment can be confirmed or rejected | `MembershipPayment.Confirm/Reject` |
| A closed ticket takes no new messages | `SupportTicket.AddMessage` |
| One registration payment, one payment per month | `CreateMembershipPaymentCommandHandler` |
| A member needs an active account and a paid fee to borrow | `CreateLoanRequestCommandHandler` |

The split is consistent: **rules that need only the aggregate live in the
entity; rules that need a database lookup live in the handler.** Uniqueness
checks are the second kind — and because a handler check loses a race, phone
number and national code also carry unique indexes at the database level.

On the penalty cap: 1,000 days late without a ceiling would be 498% — nearly six
times the principal. For an interest-free fund that is both incoherent and
self-defeating, since a debt that size guarantees the member stops paying. 48% is
a business decision, expressed as a named constant rather than a magic number.

---

## Security model

**Identity never comes from the client.** No route or request body accepts a
`memberId`. Handlers read it from `ICurrentUserService`, which reads it from the
token. A support message's `SenderType` is derived from the caller's role, so a
member cannot post as an admin.

**Money never comes from the client.** Membership payment amounts are read from
`FundSettings` server-side. A client that names its own dues can pay one rial.

**Closed by default.** `[Authorize]` sits on the controller; opening an endpoint
is a deliberate `[AllowAnonymous]`, not an omission.

**Ownership is checked in the handler**, not only at the route. Reading a loan or
a ticket you do not own returns 403. Admins are exempt from the read checks —
reviewing is the job — but **not** from `PayInstallment`:

```csharp
if (loanRequest.MemberId != _currentUserService.UserId)
{
    throw new ForbiddenException(
        "Shoma faghat mitavanid aghsat-e vam-e khodetan ra pardakht konid.");
}
```

An admin paying on a member's behalf would make the payment untraceable to the
person who made it. Reading and writing are not the same privilege.

**Login does not leak account existence.** A wrong password and an unknown phone
number return the same message, and the active-account check runs *after* the
password check — otherwise the response itself would confirm which accounts
exist.

**Passwords** use BCrypt at work factor 12. The salt lives inside the hash
string, so `PasswordHash` is the only column needed and comparison always goes
through `Verify`, never `==`.

**No secrets in the repository.** The JWT signing key, connection string, and
seed admin password live in user secrets.

---

## Testing

13 tests, all over `Domain`, none requiring a database.

The coverage is deliberately not uniform. It concentrates on the rules where
being wrong costs money or breaks trust:

- **Penalty boundaries** — days 5, 6, 11, 12, and far past due. The day-5 and
  day-11 cases are the valuable ones: they pin down that a *partial* block earns
  nothing, which is exactly where an off-by-one would hide.
- **Payment ordering** — paying instalment 2 before 1 throws.
- **Approval guards** — no guarantor, unconfirmed guarantee, self-guarantee.
- **State transitions** — confirming an already-confirmed payment, messaging a
  closed ticket, a monthly payment with no period.

Controllers and repositories have no unit tests, on purpose: controllers hold no
logic worth asserting, and repositories are EF translation, which a unit test
cannot verify meaningfully. Authorization and ownership are verified manually;
integration tests over those rules would be the next thing to write.

---

## API surface

| Area | Endpoints |
|---|---|
| Auth | register, member login, admin login |
| Members | list pending approvals, activate *(admin)* |
| Loans | create, my loans, detail, instalment schedule, pay, approve/reject *(admin)*, pending list *(admin)* |
| Guarantees | add guarantor, confirm, guarantees awaiting me |
| Membership payments | submit, my history, confirm/reject *(admin)*, pending list *(admin)* |
| Support | open ticket, reply, my tickets, detail, close *(admin)*, open tickets *(admin)* |
| Fund settings | read, update *(admin)* |

Swagger is configured to accept a bearer token, so protected endpoints are
testable from the browser.

---

## Running it locally

**Prerequisites:** .NET 10 SDK, SQL Server.

1. Clone the repository and open the solution.
2. Set user secrets on the `QarzAlHasana.Api` project — the repository ships no
   credentials:

```json
{
  "JwtSettings": {
    "Secret": "at-least-32-characters-long"
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=QarzAlHasanaDb;User Id=sa;Password=...;TrustServerCertificate=True;"
  },
  "SeedAdmin": {
    "Password": "..."
  }
}
```

3. Apply migrations.
4. Run. On startup the app seeds an initial admin and the fund's fee settings if
   they are absent. Both seeds check first, so restarting is a no-op — an admin
   who later changes the fees will not have them overwritten.
5. Open Swagger, call `POST /api/auth/admin/login`, and paste the token into
   **Authorize**.

A full walkthrough: register a member → activate them as admin → log in as the
member → submit a registration payment → confirm it as admin → request a loan →
add a guarantor → confirm as that guarantor → approve as admin → pay instalment
one.

---

## Known gaps

Listed deliberately rather than left to be found:

- User secrets are a development store. A deploy needs environment variables or a
  secret manager.
- Authorization and ownership rules are covered manually; they deserve
  integration tests.
- `IsDeleted` sits on `BaseEntity` but nothing sets it and no global query filter
  reads it. Soft delete is not implemented — the column is a placeholder, and
  wiring the filter without wiring the writes would be worse than leaving it.
- No rate limiting on login. Real deployment needs it.
- No notification layer. A guarantor learns about a pending guarantee by checking
  the endpoint, not by being told.
