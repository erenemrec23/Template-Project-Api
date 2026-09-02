using MediatR;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Feedbacks.DTOs;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Feedbacks.Queries.GetList
{
    public class GetListFeedBackQuery : PageRequestBaseDto, IRequest<Result<Paginate<FeedBackListItemDto>>>;


}
