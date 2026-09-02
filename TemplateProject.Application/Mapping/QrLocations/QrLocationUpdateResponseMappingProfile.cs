using AutoMapper;
using QrAssignment.Application.Features.QrLocations.Commands.Update;
using QrAssignment.Domain.Entity;
using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Application.Mapping.QrLocations
{
    public class QrLocationUpdateResponseMappingProfile : Profile
    {
        public QrLocationUpdateResponseMappingProfile()
        {
            CreateMap<UpdateQrLocationCommand, QrLocation>();
            CreateMap<QrLocation, UpdateQrLocationResponse>();
        }
    }
}
