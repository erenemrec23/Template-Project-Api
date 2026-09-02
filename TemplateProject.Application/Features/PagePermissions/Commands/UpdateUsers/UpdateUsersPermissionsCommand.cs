// Application/Features/PagePermissions/Commands/UpdateUsersPermissions/UpdateUsersPermissionsCommand.cs
using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Features.PagePermissions.DTOs;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.PagePermissions.Commands.UpdateUsersPermissions
{
    // Çoklu kullanıcıya aynı yetki setini uygular (bulk). Targeted sync (PermissionSyncService).
    public sealed record UpdateUsersPermissionsCommand(
        List<Guid> UserIds,
        List<PermissionAssignmentDto> Permissions)
        : ICommand<Result>;
}