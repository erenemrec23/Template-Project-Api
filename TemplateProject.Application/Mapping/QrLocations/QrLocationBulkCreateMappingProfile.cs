using AutoMapper;
using QrAssignment.Application.Features.QrLocations.Commands.Excel.BulkCreate;
using QrAssignment.Domain.Entity;
using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Application.Mapping.QrLocations
{
    public class QrLocationBulkCreateMappingProfile : Profile
    {
        public QrLocationBulkCreateMappingProfile()
        {
            CreateMap<BulkCreateQrLocationInputDto, QrLocation>();
        }
    }
}
