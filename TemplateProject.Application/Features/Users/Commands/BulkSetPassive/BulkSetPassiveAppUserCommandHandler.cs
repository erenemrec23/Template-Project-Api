using MediatR;
using QrAssignment.Application.Features.Users.Commands.BulkSetPassive;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Commands.BulkSetPassive
{
    public class BulkSetPassiveAppUserCommandHandler : IRequestHandler<BulkSetPassiveAppUserCommand, Result>
    {
        private readonly IAppUserRepository _appUserRepository;

        public BulkSetPassiveAppUserCommandHandler(IAppUserRepository appUserRepository)
            => _appUserRepository = appUserRepository;

        public async Task<Result> Handle(BulkSetPassiveAppUserCommand request, CancellationToken cancellationToken)
        {
            await _appUserRepository.BulkDeleteAsync(request.IdList, cancellationToken);
            return Result.Success();
        }
    }
}
