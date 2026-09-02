using MediatR;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Tenants.Queries.DTOs;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Queries.ListBase.GetPassivedList
{
    public class GetPassiveListTenantQueryHandler : IRequestHandler<GetPassivedListTenantQuery, Result<Paginate<TenantListItemDto>>>
    {
        private readonly ITenantRepository _tenantRepository;

        public GetPassiveListTenantQueryHandler(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        public async Task<Result<Paginate<TenantListItemDto>>> Handle(GetPassivedListTenantQuery request, CancellationToken cancellationToken)
        {
            var result = await _tenantRepository.GetPassivedDtoListAsync(request, cancellationToken);

            return Result.Success(result);
        }

    }
}
