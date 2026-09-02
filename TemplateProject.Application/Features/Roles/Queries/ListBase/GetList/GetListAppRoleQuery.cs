using MediatR;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Roles.Queries.GetList;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Queries.ListBase.GetList
{

    // Query
    // Result nesnesinin generic versiyonu (Result<T>) ile data dönüyoruz
    public sealed class GetListAppRoleQuery : PageRequestBaseDto, IRequest<Result<Paginate<RoleListItemDto>>>;
}