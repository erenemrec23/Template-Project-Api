using AutoMapper;
using QrAssignment.Application.Features.QrLocations.Commands.Update;
using QrAssignment.Domain.Entity;

namespace QrAssignment.Application.Mapping.QrLocations
{
    public class QrLocationUpdateMappingProfile : Profile
    {
        public QrLocationUpdateMappingProfile()
        {
            //CreateMap<QrLocation, UpdateQrLocationCommand>()
            //    .ForMember(dest => dest.ParentLocationId, opt =>
            //        opt.MapFrom(src => src.ParentLocation != null ? src.ParentLocation.Id : (Guid?)null));

            //CreateMap<UpdateQrLocationCommand, QrLocation>()
            //    .ForMember(dest => dest.Id, opt => opt.Ignore())
            //    .ForMember(dest => dest.ParentLocation, opt => opt.Ignore())
            //    .ForMember(dest => dest.SubLocations, opt => opt.Ignore());
        }
    }
}