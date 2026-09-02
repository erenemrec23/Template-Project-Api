// Application/Features/Permission/Commands/UpdatePagePermissionsForPage/UpdatePagePermissionsForPageCommand.cs
using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Features.PagePermissions.DTOs;
using QrAssignment.Application.Security;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.PagePermissions.Commands.Update
{
    public sealed record UpdatePagePermissionsForPageCommand(
        string PageKey,  
        List<PermissionAssignmentDto> Permissions,
        Guid? UserId,
        Guid? RoleId)
        : IPageScopedRequest, ICommand<Result>;

}