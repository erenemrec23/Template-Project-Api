using MediatR;
using QrAssignment.Application.Features.Roles.Commands.BulkDelete;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Commands.BulkSetPassive
{
    public class BulkSetPassiveAppRoleCommandHandler : IRequestHandler<BulkSetPassiveAppRoleCommand, Result>
    {
        private readonly IAppRoleRepository _appRoleRepository;
        public BulkSetPassiveAppRoleCommandHandler(IAppRoleRepository appRoleRepository)
            => _appRoleRepository = appRoleRepository;

        public async Task<Result> Handle(BulkSetPassiveAppRoleCommand request, CancellationToken cancellationToken)
        {
            await _appRoleRepository.BulkSetPassiveByIds(request.IdList, cancellationToken);
            return Result.Success();
        }
    }
}