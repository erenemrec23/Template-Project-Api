// Application/Features/PagePermissions/Commands/UpdateRolesPermissions/UpdateRolesPermissionsCommandHandler.cs
using MediatR;
using QrAssignment.Application.Features.Permission.Commands.Update; // PermissionUserUpdateDto
using QrAssignment.Application.Services;                            // IPermissionSyncService
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.PagePermissions.Commands.UpdateRolesPermissions
{
    public sealed class UpdateRolesPermissionsCommandHandler
        : IRequestHandler<UpdateRolesPermissionsCommand, Result>
    {
        private readonly IPermissionSyncService _permissionSyncService;

        public UpdateRolesPermissionsCommandHandler(IPermissionSyncService permissionSyncService)
            => _permissionSyncService = permissionSyncService;

        public async Task<Result> Handle(UpdateRolesPermissionsCommand request, CancellationToken cancellationToken)
        {
            var permissions = request.Permissions.Select(s => new PermissionUserUpdateDto
            {
                PageName = s.PageName,
                GroupKey = s.GroupKey,
                PermissionValue = s.PermissionValue
            });

            await _permissionSyncService.SyncRolesPermissionsAsync(request.RoleIds, permissions, cancellationToken);

            return Result.Success();
        }
    }
}