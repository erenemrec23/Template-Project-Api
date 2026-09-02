using MediatR;
using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Features.Roles.DTOs;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Queries.FormBase.GetById
{
    public class GetByIdRoleQuery : IRequest<Result<RoleItemDto>>, IdValidationBase
    {
        public Guid? Id { get; set; }
        public GetByIdRoleQuery(Guid? id) => Id = id;
    }
}