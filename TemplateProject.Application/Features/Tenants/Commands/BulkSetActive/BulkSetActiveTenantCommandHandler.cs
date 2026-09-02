using AutoMapper;
using MediatR;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Commands.BulkSetActive
{
    public class BulkSetActiveTenantCommandHandler : IRequestHandler<BulkSetActiveTenantCommand, Result>
    {
        private readonly ITenantRepository _tenantRepository; 

        public BulkSetActiveTenantCommandHandler(ITenantRepository tenantRepository, IMapper mapper, IAppLocalizer localizer)
        {
            _tenantRepository = tenantRepository; 
        }

        public async Task<Result> Handle(BulkSetActiveTenantCommand request, CancellationToken cancellationToken)
        {  
            await _tenantRepository.BulkSetActiveByIdsAsync(request.IdList, cancellationToken); 
            return Result.Success();
        }
    }
}