using MediatR;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.QrLocations.Queries.DTOs;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.QrLocations.Queries.ListBase.GetList
{
    public class GetListQrLocationQuery : PageRequestBaseDto, IRequest<Result<Paginate<QrLocationListItemDto>>>
    {

    }
}
