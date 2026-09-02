using AutoMapper;
using MediatR; 
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Commands.SetActive
{
    public class SetActiveTenantCommandHandler : IRequestHandler<SetActiveTenantCommand, Result>
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IMapper _mapper;
        private readonly IAppLocalizer _localizer;

        public SetActiveTenantCommandHandler(ITenantRepository tenantRepository, IMapper mapper, IAppLocalizer localizer)
        {
            _tenantRepository = tenantRepository;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result> Handle(SetActiveTenantCommand request, CancellationToken cancellationToken)
        {
            if (!request.Id.HasValue)
                throw new Exception(_localizer["Messages.IdIsNull"]);

            var tenant = await _tenantRepository.GetPassivedByIdAsync(request.Id.Value, cancellationToken);

            if (tenant == null)
                throw new Exception(_localizer["Messages.TenantNotFound"]);

            tenant.IsPassived = false;

            _tenantRepository.Update(tenant);
             
            return Result.Success();
        }
    }
}