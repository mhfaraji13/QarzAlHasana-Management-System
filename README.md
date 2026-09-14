[README.md](https://github.com/user-attachments/files/32174686/README.md)
# QarzAlHasana — Interest-Free Loan Fund

A backend API for a *qarz al-hasana* fund: a community savings pool where members
pay dues, request interest-free loans, and repay them in instalments. Built with
.NET 10, Clean Architecture, CQRS, and EF Core.

This is a portfolio project. The business rules are small enough to read in one
sitting and real enough that getting them wrong costs someone money — which is
what makes the design decisions below worth explaining.

---

## What it does

- **Membership** — members register, an admin approves them, and dues are paid
  against a receipt that an admin confirms or rejects.
- **Loans** — an approved member requests a loan; an admin approves it; an
  instalment schedule is generated automatically.
- **Penalties** — late instalments accrue 3% per completed 6-day block, capped
  at 48%.
- **Support tickets** — members open tickets, admins reply and close them.
- **Auth** — JWT with two roles, `Member` and `Admin`.

---

## Architecture decisions

These are the decisions worth defending, and why.

### Store the fact, compute the opinion

`InstallmentStatus` has two values, `Unpaid` and `Paid`. There is no `Overdue`.

Whether an instalment is late is derivable from `DueDate` and `Status`, so it is
computed — `IsOverdue(asOf)` — not stored. A stored `Overdue` flag would need a
background job to keep it truthful, and would be wrong between runs.

The penalty is the opposite. `PenaltyAmount` and `PaidAmount` are frozen onto the
instalment at the moment of payment, because once money changes hands the figure
is a fact that happened, not a view of the present. Recomputing it later would
let a rule change rewrite history.

### The compiler guards the aggregate root

`Installment.Pay(...)` is `internal`. The Application layer is a separate
assembly, so it cannot call it. The only way in is
`LoanRequest.PayInstallment(...)`.

That matters because two rules live on the aggregate root and would be trivially
bypassed by calling the instalment directly: instalments must be paid oldest
first, and the amount must equal instalment plus penalty exactly — no partial
payments, no paying the penalty separately.

A rule the compiler enforces cannot be forgotten in review.

### Time is a parameter, not a call

No entity reads `DateTime.UtcNow` inside its penalty logic. `asOf` is passed in:

```csharp
installment.CalculatePenalty(dueDate.AddDays(6));   // 3% of 1000 = 30
installment.CalculatePenalty(dueDate.AddDays(11));  // still 30 — block not complete
installment.CalculatePenalty(dueDate.AddDays(12));  // 60
installment.CalculatePenalty(dueDate.AddDays(1000));// 480 — capped at 48%
```

Those four lines are the unit tests. With `UtcNow` buried in the entity, testing
"assume six days have passed" would mean changing the system clock.

The 48% cap is a business decision, not a rounding artefact: 1000 days late would
otherwise mean 498% — nearly six times the principal, which defeats the purpose
of an interest-free fund and guarantees the member stops paying.

### Two gates, not one

`IsActive` and `HasPaidRegistrationFee` look similar and are deliberately
separate.

`IsActive` answers *are you who you say you are* — an admin checks the national
code and phone number before the account can log in at all.
`HasPaidRegistrationFee` answers *are you financially a member* — it gates loan
requests, nothing else.

Collapsing them creates a deadlock: a member cannot record a payment without
logging in, and cannot log in without having paid.

### The client never sends identity or money

`memberId` does not appear in any route or request body. Handlers read it from
`ICurrentUserService`, which reads it from the token. A support message's
`SenderType` is derived from the token role, so a member cannot post as an admin.

Payment amounts come from `FundSettings` on the server. A client that can name
its own dues can pay one rial.

Ownership is checked in the handler, not just the controller: reading a loan or a
ticket you do not own returns 403. Admins are exempt from the read checks, since
reviewing is their job — but **not** from `PayInstallment`. An admin paying on a
member's behalf would make the payment untraceable to the person who made it.

### Missing configuration is an error, not a default

`GetRegistrationFeeAsync` throws when `FundSettings` is empty. It used to return
`0`, which meant every membership payment was silently recorded as free. Nothing
threw; the data was just wrong.

That is the same failure mode as a missing `Include`: the query returned an empty
`Installments` collection, so `All(i => i.Status == Paid)` was vacuously true and
the loan was marked `Paid` without a single payment. No exception, a confidently
wrong answer.

Both are fixed. Both are the reason this codebase prefers throwing over
defaulting.

### CQRS, and what that buys

Write paths load entities and enforce rules. Read paths project straight to DTOs
inside the repository, with `AsNoTracking`, so EF emits a `SELECT` with only the
needed columns and no `Include` is required.

DTOs are per audience, not per entity. A member's payment history carries the
rejection reason so they know what to fix; the admin's pending list does not. The
admin's ticket list carries the member's name; the member's own list does not.

---

## Stack

| Layer | Contents |
|---|---|
| `Domain` | Entities, enums, business rules, `BusinessRuleException` |
| `Application` | Commands, queries, handlers, validators, repository interfaces |
| `Infrastructure` | EF Core, repositories, JWT generation, BCrypt hashing |
| `Api` | Controllers, contracts, exception handlers, `CurrentUserService` |
| `Domain.Tests` | xUnit tests over the penalty and payment-ordering rules |

MediatR for dispatch, FluentValidation for input shape, BCrypt (work factor 12)
for passwords, SQL Server via EF Core.

Errors map to status codes through `IExceptionHandler` implementations:
validation → 400, unauthenticated → 401, ownership → 403, missing → 404, broken
business rule → 409.

---

## Running it

**Prerequisites:** .NET 10 SDK, SQL Server.

1. Clone the repository and open the solution.
2. Set user secrets on the `QarzAlHasana.Api` project — the repository contains
   no credentials:

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
   they are absent — both seeds are no-ops once the rows exist.
5. Open Swagger, call `POST /api/auth/admin/login`, and paste the token into
   **Authorize**.

---

## Known gaps

Listed deliberately rather than left to be discovered:

- User secrets are a development store. A deploy needs environment variables or
  a secret manager.
- `Guarantor` exists as an entity but has no commands or queries; loans can
  currently be requested without one.
- Test coverage stops at the domain rules that move money. Membership payments,
  tickets, and authorization have no tests.
- `IsDeleted` sits on `BaseEntity` but nothing sets it and no global query filter
  reads it. Soft delete is not implemented — the column is a placeholder.
