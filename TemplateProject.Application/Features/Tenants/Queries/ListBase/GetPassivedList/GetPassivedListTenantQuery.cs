using MediatR;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Tenants.Queries.DTOs;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Queries.ListBase.GetPassivedList
{
    public class GetPassivedListTenantQuery : PageRequestBaseDto, IRequest<Result<Paginate<TenantListItemDto>>>
    {

    }

}
