using AutoMapper;
using MediatR;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Commands.BulkSetPassive
{
    public class BulkSetPassiveTenantCommandHandler : IRequestHandler<BulkSetPassiveTenantCommand, Result>
    {
        private readonly ITenantRepository _tenantRepository; 

        public BulkSetPassiveTenantCommandHandler(ITenantRepository tenantRepository, IMapper mapper, IAppLocalizer localizer)
        {
            _tenantRepository = tenantRepository; 
        }

        public async Task<Result> Handle(BulkSetPassiveTenantCommand request, CancellationToken cancellationToken)
        {  
            await _tenantRepository.BulkSetPassiveByIdsAsync(request.IdList, cancellationToken); 
            return Result.Success();
        }
    }
}