using MediatR;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Tenants.DTOs;
using QrAssignment.Application.Features.Tenants.Queries.DTOs;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Queries.ListBase.GetList
{
    public class GetListTenantQueryHandler : IRequestHandler<GetListTenantQuery, Result<Paginate<TenantListItemDto>>>
    {
        private readonly ITenantRepository _tenantRepository;

        public GetListTenantQueryHandler(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        public async Task<Result<Paginate<TenantListItemDto>>> Handle(GetListTenantQuery request, CancellationToken cancellationToken)
        {
            var result = await _tenantRepository.GetDtoListAsync(request, cancellationToken);

            return Result.Success(result);
        }

    }
}
