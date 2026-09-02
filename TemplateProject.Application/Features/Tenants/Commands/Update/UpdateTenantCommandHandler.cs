using AutoMapper;
using MediatR;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Commands.Update
{
    public class UpdateTenantCommandHandler : IRequestHandler<UpdateTenantCommand, Result<UpdateTenantResponse>>
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IMapper _mapper;
        private readonly IAppLocalizer _localizer;
        public UpdateTenantCommandHandler(ITenantRepository tenantRepository, IMapper mapper, IAppLocalizer localizer)
        {
            _tenantRepository = tenantRepository;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<UpdateTenantResponse>> Handle(UpdateTenantCommand request, CancellationToken cancellationToken)
        {
            if (!request.Id.HasValue)
                throw new Exception(_localizer["Messages.IdIsNull"]);
            var tenant = await _tenantRepository.GetByIdAsync(request.Id.Value, cancellationToken);

            if (tenant == null)
                throw new Exception(_localizer["Messages.TenantNotFound"]);

            _mapper.Map(request, tenant);

            //await _tenantRepository.Update(tenant, cancellationToken);

            _tenantRepository.Update(tenant);

            var response = new UpdateTenantResponse();
            _mapper.Map(tenant, response);

            return Result.Success(response);
        }
    }
}
