using AutoMapper;
using QrAssignment.Application.Features.Tenants.Commands.Update;
using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Application.Mapping.Tenants
{
    public class TenantUpdateResponseMappingProfile : Profile
    {
        public TenantUpdateResponseMappingProfile()
        {

            CreateMap<UpdateTenantCommand, Tenant>();
            CreateMap<Tenant, UpdateTenantResponse>();
        }
    }

}  