using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Features.Permission.Commands.Update; // PermissionUserUpdateDto
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Commands.Create
{
    public sealed record CreateAppUserCommand(
        string FirstName,
        string LastName,
        string Email,
        string Password,
        // Sayfa/grup yetkileri (null => dokunma; boş liste => yetki yok)
        List<PermissionUserUpdateDto>? Permissions = null,
        // Atanacak roller (null => dokunma; boş liste => rolsüz)
        List<Guid>? RoleIds = null) : ICommand<Result>;
}