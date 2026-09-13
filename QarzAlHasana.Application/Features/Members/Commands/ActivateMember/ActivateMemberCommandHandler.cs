using MediatR;
using QarzAlHasana.Application.Common.Interfaces;
using QarzAlHasana.Application.Common.Interfaces.Repositories;
using QarzAlHasana.Domain.Exceptions;
using QarzAlHasanaSystem.Application.Common.Exceptions;

namespace QarzAlHasana.Application.Features.Members.Commands.ActivateMember;

public class ActivateMemberCommandHandler
    : IRequestHandler<ActivateMemberCommand>
{
    private readonly IMemberRepository _memberRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ActivateMemberCommandHandler(
        IMemberRepository memberRepository,
        IUnitOfWork unitOfWork)
    {
        _memberRepository = memberRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        ActivateMemberCommand request,
        CancellationToken cancellationToken)
    {
        var member = await _memberRepository
            .GetByIdAsync(request.MemberId, cancellationToken);

        if (member is null)
        {
            throw new NotFoundException(
                $"Ozv ba shenase {request.MemberId} peyda nashod.");
        }

        member.Activate();

        _memberRepository.Update(member);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}