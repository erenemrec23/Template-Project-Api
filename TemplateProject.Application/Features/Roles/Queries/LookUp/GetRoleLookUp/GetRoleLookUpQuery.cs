using MediatR;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Users.Queries.LookUp.DTOs;
using QrAssignment.Domain.Shared;


namespace QrAssignment.Application.Features.Roles.Queries.LookUp.GetRoleLookUp
{
    public sealed class GetRoleLookUpQuery
        : IRequest<Result<List<RoleLookUpListItemDto>>>;
}
