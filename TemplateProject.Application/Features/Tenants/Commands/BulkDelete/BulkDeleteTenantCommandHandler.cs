using AutoMapper;
using MediatR;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Commands.BulkDelete
{
    public class BulkDeleteTenantCommandHandler : IRequestHandler<BulkDeleteTenantCommand, Result>
    {
        private readonly ITenantRepository _tenantRepository; 

        public BulkDeleteTenantCommandHandler(ITenantRepository tenantRepository, IMapper mapper, IAppLocalizer localizer)
        {
            _tenantRepository = tenantRepository; 
        }

        public async Task<Result> Handle(BulkDeleteTenantCommand request, CancellationToken cancellationToken)
        {  
            await _tenantRepository.DeleteRange(request.IdList, cancellationToken); 
            return Result.Success();
        }
    }
}