using MediatR;
using QrAssignment.Application.Features.Users.Queries.LookUp.DTOs;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Queries.LookUp.GetLookupList
{
    public class GetLookUpListAppUserQuery : IRequest<Result<List<AppUserLookUpListItemDto>>>;
    
}
