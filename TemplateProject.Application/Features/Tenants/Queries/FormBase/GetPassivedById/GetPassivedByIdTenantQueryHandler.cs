using MediatR;
using QrAssignment.Application.Features.Tenants.DTOs;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Queries.FormBase.GetPassivedById
{
    public class GetPassivedByIdTenantQueryHandler : IRequestHandler<GetPassivedByIdTenantQuery, Result<TenantItemDto>>
    {


        private readonly ITenantRepository _tenantRepository;

        public GetPassivedByIdTenantQueryHandler(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        public async Task<Result<TenantItemDto>> Handle(GetPassivedByIdTenantQuery request, CancellationToken cancellationToken)
        {
            var result = await _tenantRepository.GetPassivedDtoByIdAsync(request.Id.Value, cancellationToken);

            return Result.Success(result);
        }
    }
}
