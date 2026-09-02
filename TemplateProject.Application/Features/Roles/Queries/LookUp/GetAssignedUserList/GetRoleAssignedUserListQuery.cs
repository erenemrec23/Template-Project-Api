using MediatR;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Queries.LookUp.GetAssignedUserList
{
    public class GetRoleAssignedUserListQuery : PageRequestBaseDto, IRequest<Result<List<Guid>>>
    {
        public GetRoleAssignedUserListQuery() { }
        public GetRoleAssignedUserListQuery(Guid? roleId) {
            RoleId = roleId;

        }

        public Guid? RoleId { get; set; }
    }
}
