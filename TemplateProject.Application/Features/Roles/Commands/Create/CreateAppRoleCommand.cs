using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Features.Roles.Commands.DTOs;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Commands.Create
{
    public sealed record CreateAppRoleCommand(
        string Name,
        List<RolePagePermissionDto> Permissions,
        List<Guid> UserIds
    ) : ICommand<Result>;
}