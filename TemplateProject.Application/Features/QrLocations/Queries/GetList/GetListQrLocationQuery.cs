using MediatR;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.QrLocations.Queries.DTOs;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.QrLocations.Queries.GetList
{
    public class GetListQrLocationQuery : PageRequestBaseDto, IRequest<Result<Paginate<QrLocationListItemDto>>>;


}
