using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Features.Permission.Commands.Update; // PermissionUserUpdateDto
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Commands.Update
{
    public sealed record UpdateAppUserCommand(
        Guid? Id,
        string FirstName,
        string LastName,
        string Email, 
        List<PermissionUserUpdateDto>? Permissions = null, 
        List<Guid>? RoleIds = null) : ICommand<Result>, IdValidationBase;
}