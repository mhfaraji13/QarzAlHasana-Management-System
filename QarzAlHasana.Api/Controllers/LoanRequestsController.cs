using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QarzAlHasana.Application.Features.LoanRequests.Commands.CreateLoanRequest;
using QarzAlHasana.Application.Features.LoanRequests.Queries.GetMemberLoanRequests;

namespace QarzAlHasana.Api.Controllers
{
    [Route("api/loan-requests")]
    [ApiController]
    public class LoanRequestsController : ControllerBase
    {
        private readonly ISender _sender;

        public LoanRequestsController(ISender sender)
        {
            _sender = sender;
        }
        
        
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateLoanRequestCommand command,
            CancellationToken cancellationToken)
        {
            var id = await _sender.Send(command, cancellationToken);
            return Ok(new { id });
        }

        [HttpGet("member/{memberId:guid}")]
        public async Task<IActionResult> GetByMember(
            Guid memberId,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetMemberLoanRequestsQuery(memberId), cancellationToken);
            return Ok(result);
        }
    }
}
