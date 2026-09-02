using AutoMapper;
using QrAssignment.Application.Features.QrLocations.Commands.Create;
using QrAssignment.Domain.Entity;

namespace QrAssignment.Application.Mapping.QrLocations
{
    public class QrLocationCreateMappingProfile : Profile
    {
        public QrLocationCreateMappingProfile()
        {
            CreateMap<CreateQrLocationCommand, QrLocation>(); 
        }
    }
}
