using AutoMapper;
using MediatR;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;
using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Application.Features.Tenants.Commands.Create
{
    public class CreateTenantCommandHandler : IRequestHandler<CreateTenantCommand, Result<Guid>>
    {
        private readonly IMapper _mapper;
        private readonly ITenantRepository _tenantRepository;
        public CreateTenantCommandHandler(ITenantRepository tenantRepository, IMapper mapper)
        {
            _mapper = mapper;
            _tenantRepository = tenantRepository;
        }

        public async Task<Result<Guid>> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
        {

            var tenant = _mapper.Map<Tenant>(request);
            await _tenantRepository.AddAsync(tenant, cancellationToken);
            return Result.Success(tenant.Id);
        }
    }
}
