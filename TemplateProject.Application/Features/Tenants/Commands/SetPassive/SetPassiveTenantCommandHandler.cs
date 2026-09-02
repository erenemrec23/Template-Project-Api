using AutoMapper;
using MediatR;
using QrAssignment.Application.Features.Tenants.Commands.SetPassive;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Commands.SetActive
{
    public class SetPassiveTenantCommandHandler : IRequestHandler<SetPassiveTenantCommand, Result>
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IMapper _mapper;
        private readonly IAppLocalizer _localizer;

        public SetPassiveTenantCommandHandler(ITenantRepository tenantRepository, IMapper mapper, IAppLocalizer localizer)
        {
            _tenantRepository = tenantRepository;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result> Handle(SetPassiveTenantCommand request, CancellationToken cancellationToken)
        {
            //if (!request.Id.HasValue)
            //    throw new Exception(_localizer["Messages.IdIsNull"]);

            //var tenant = await _tenantRepository.GetByIdAsync(request.Id.Value, cancellationToken);

            //if (tenant == null)
            //    throw new Exception(_localizer["Messages.TenantNotFound"]); 
            await _tenantRepository.SetPassiveByIdAsync(request.Id.Value,cancellationToken);
             
            return Result.Success();
        }
    }
}