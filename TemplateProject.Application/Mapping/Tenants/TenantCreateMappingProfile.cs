using AutoMapper;
using QrAssignment.Application.Features.QrLocations.Commands.Create;
using QrAssignment.Application.Features.Tenants.Commands.Create;
using QrAssignment.Domain.Entity;
using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Application.Mapping.Tenants
{
    public class TenantCreateMappingProfile : Profile
    {
        public TenantCreateMappingProfile()
        {
            CreateMap<CreateTenantCommand , Tenant>();
        }
    }

}  