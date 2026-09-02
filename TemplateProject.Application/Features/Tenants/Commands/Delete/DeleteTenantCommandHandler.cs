using AutoMapper;
using MediatR;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Commands.Delete
{
    public class DeleteTenantCommandHandler : IRequestHandler<DeleteTenantCommand, Result>
    {
        private readonly ITenantRepository _tenantRepository; 
        private readonly IAppLocalizer _localizer;

        public DeleteTenantCommandHandler(ITenantRepository tenantRepository, IMapper mapper, IAppLocalizer localizer)
        {
            _tenantRepository = tenantRepository; 
            _localizer = localizer;
        }

        public async Task<Result> Handle(DeleteTenantCommand request, CancellationToken cancellationToken)
        { 
            if (!request.Id.HasValue)
                throw new Exception(_localizer["Messages.IdIsNull"]);
             
            var tenant = await _tenantRepository.GetByIdAsync(request.Id.Value, cancellationToken);

            if (tenant == null)
                throw new Exception(_localizer["Messages.TenantNotFound"]);
             
            _tenantRepository.Delete(tenant); 
            return Result.Success();
        }
    }
}