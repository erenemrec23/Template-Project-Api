using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;
using MediatR;

namespace QrAssignment.Application.Features.Users.Commands.BulkSetActive
{
    public class BulkSetActiveAppUserCommandHandler : IRequestHandler<BulkSetActiveAppUserCommand, Result>
    {
        private readonly IAppUserRepository _appUserRepository;

        public BulkSetActiveAppUserCommandHandler(IAppUserRepository appUserRepository)
            => _appUserRepository = appUserRepository;

        public async Task<Result> Handle(BulkSetActiveAppUserCommand request, CancellationToken cancellationToken)
        {
            await _appUserRepository.BulkSetActiveByIds(request.IdList, cancellationToken);
            return Result.Success();
        }
    }
}
