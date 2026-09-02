using MediatR;
using QrAssignment.Domain.Shared;
using QrAssignment.Application.Features.Menu.Queries.DTOs;

namespace QrAssignment.Application.Features.Menu.Queries.GetUserList
{
    // Giris yapan kullaniciya gore filtrelenmis menu. Parametre yok;
    // kullanici bilgisi token claim'lerinden okunur (ICurrentUserService).
    public sealed record GetUserMenuQuery : IRequest<Result<List<MenuGroupDto>>>;
}