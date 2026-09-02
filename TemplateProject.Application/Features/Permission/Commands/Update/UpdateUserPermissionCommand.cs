using QrAssignment.Application.Abstractions;
using QrAssignment.Domain.Shared; 

namespace QrAssignment.Application.Features.Permission.Commands.Update
{
    public sealed record UpdateUserPermissionCommand(
    string UserId,
    List<PermissionUserUpdateDto> Permissions) : ICommand<Result>;
}
