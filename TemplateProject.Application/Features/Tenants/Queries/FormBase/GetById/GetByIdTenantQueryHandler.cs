using MediatR;
using QrAssignment.Application.Features.Tenants.DTOs;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Queries.FormBase.GetById
{
    public class GetByIdTenantQueryHandler : IRequestHandler<GetByIdTenantQuery, Result<TenantItemDto>>
    {


        private readonly ITenantRepository _tenantRepository;

        public GetByIdTenantQueryHandler(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        public async Task<Result<TenantItemDto>> Handle(GetByIdTenantQuery request, CancellationToken cancellationToken)
        {
            var result = await _tenantRepository.GetDtoByIdAsync(request.Id.Value, cancellationToken);

            return Result.Success(result);
        }
    }
}
