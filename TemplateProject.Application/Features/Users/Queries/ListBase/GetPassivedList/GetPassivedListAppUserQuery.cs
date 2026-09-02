using QrAssignment.Application.Abstractions;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Users.Queries.DTOs;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Queries.ListBase.GetPassivedList
{
    public class GetPassivedListAppUserQuery : PageRequestBaseDto, ICommand<Result<Paginate<AppUserListItemDto>>>
    {
    }
}
