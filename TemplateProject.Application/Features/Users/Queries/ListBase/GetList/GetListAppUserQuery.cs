using QrAssignment.Application.Abstractions;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Users.Queries.DTOs;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Queries.ListBase.GetList
{
    // AppRole'daki GetListAppRoleQuery gibi PageRequestBaseDto tasiyor (filtre + sayfalama)
    // ve POST body ile geliyor.
    public class GetListAppUserQuery : PageRequestBaseDto, ICommand<Result<Paginate<AppUserListItemDto>>>
    {
    }
}
