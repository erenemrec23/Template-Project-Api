using MediatR;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Queries.LookUp.GetLookUpListAppUserAssignedRoleId
{
    public class GetLookUpListAppUserAssignedRoleIdQuery : IRequest<Result<List<Guid>>>
    {
        public GetLookUpListAppUserAssignedRoleIdQuery(Guid? userId)
        {
            UserId = userId;
        }

        public Guid? UserId { get; set; }
    }

}
