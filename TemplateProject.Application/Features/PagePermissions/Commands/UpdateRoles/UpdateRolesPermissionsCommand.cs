// Application/Features/PagePermissions/Commands/UpdateRolesPermissions/UpdateRolesPermissionsCommand.cs
using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Features.PagePermissions.DTOs;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.PagePermissions.Commands.UpdateRolesPermissions
{
    // Çoklu role aynı yetki setini uygular (bulk). Targeted sync (PermissionSyncService).
    public sealed record UpdateRolesPermissionsCommand(
        List<Guid> RoleIds,
        List<PermissionAssignmentDto> Permissions)
        : ICommand<Result>;
}