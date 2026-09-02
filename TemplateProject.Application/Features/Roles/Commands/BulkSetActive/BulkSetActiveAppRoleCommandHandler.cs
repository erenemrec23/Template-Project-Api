using MediatR;
using QrAssignment.Application.Features.Roles.Commands.BulkDelete;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Commands.BulkSetActive
{
    public class BulkSetActiveAppRoleCommandHandler : IRequestHandler<BulkSetActiveAppRoleCommand, Result>
    {
        private readonly IAppRoleRepository _appRoleRepository;
        public BulkSetActiveAppRoleCommandHandler(IAppRoleRepository appRoleRepository)
            => _appRoleRepository = appRoleRepository;

        public async Task<Result> Handle(BulkSetActiveAppRoleCommand request, CancellationToken cancellationToken)
        {
            await _appRoleRepository.BulkSetActiveAsync(request.IdList, cancellationToken);
            return Result.Success();
        }
    }
}