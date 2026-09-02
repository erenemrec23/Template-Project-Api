using MediatR;
using QrAssignment.Domain.Shared;
using QrAssignment.Application.Features.Menu.Queries.DTOs;

namespace QrAssignment.Application.Features.Menu.Queries.GetList
{

    public sealed record GetListMenuQuery : IRequest<Result<List<MenuGroupDto>>>;
}
