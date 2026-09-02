using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Features.Permission.Commands.Update;
using QrAssignment.Application.Features.Roles.Commands.DTOs;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Commands.Update
{
    public sealed record UpdateAppRoleCommand(
        Guid? Id,
        string Name,
        List<PermissionUserUpdateDto> Permissions,
        List<Guid> UserIds
    ) : ICommand<Result>, IdValidationBase;
}