using MediatR; 
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Queries.GetAssignedPermissionList
{
    public class GetRoleAssignedPermissionListQuery : IRequest<Result<RolePermissionDto>>
    {
        public GetRoleAssignedPermissionListQuery() { }
        public GetRoleAssignedPermissionListQuery(Guid? roleId)
        {
            RoleId = roleId;
        }

        public Guid? RoleId { get; set; }
    }
}