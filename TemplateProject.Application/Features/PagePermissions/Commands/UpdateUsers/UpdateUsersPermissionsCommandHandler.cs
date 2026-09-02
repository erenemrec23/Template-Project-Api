// Application/Features/PagePermissions/Commands/UpdateUsersPermissions/UpdateUsersPermissionsCommandHandler.cs
using MediatR;
using QrAssignment.Application.Features.Permission.Commands.Update; // PermissionUserUpdateDto
using QrAssignment.Application.Services;                            // IPermissionSyncService
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.PagePermissions.Commands.UpdateUsersPermissions
{
    public sealed class UpdateUsersPermissionsCommandHandler
        : IRequestHandler<UpdateUsersPermissionsCommand, Result>
    {
        private readonly IPermissionSyncService _permissionSyncService;

        public UpdateUsersPermissionsCommandHandler(IPermissionSyncService permissionSyncService)
            => _permissionSyncService = permissionSyncService;

        public async Task<Result> Handle(UpdateUsersPermissionsCommand request, CancellationToken cancellationToken)
        {
            var permissions = request.Permissions.Select(s => new PermissionUserUpdateDto
            {
                PageName = s.PageName,
                GroupKey = s.GroupKey,
                PermissionValue = s.PermissionValue
            });

            await _permissionSyncService.SyncUsersPermissionsAsync(request.UserIds, permissions, cancellationToken);

            return Result.Success();
        }
    }
}