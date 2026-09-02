
// Application/Features/Permission/Commands/UpdatePagePermissionsForPage/UpdatePagePermissionsForPageCommand2.cs
using MediatR;
using QrAssignment.Application.Features.PagePermissions.DTOs;
using QrAssignment.Application.Security;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.PagePermissions.Commands.Update2
{
    public sealed record UpdatePagePermissionsForPageCommand2(
        string PageKey,
        List<PermissionAssignmentDto2> Permissions)
        : IPageScopedRequest, IRequest<Result>;

}