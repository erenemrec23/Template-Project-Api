using MediatR;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Permission.Queries.GetByUserId
{
    
    public class GetByIdPermissionUserQuery : IRequest<Result<PermissionUserItemDto>>
    {

        public Guid? UserId { get; set; }

        public GetByIdPermissionUserQuery(Guid? userId)
        {
            UserId = userId;
        }
    }
}
