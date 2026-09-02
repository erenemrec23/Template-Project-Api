using MediatR;
using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Features.Roles.DTOs;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Queries.FormBase.GetPassivedById
{
    public class GetPassivedByIdAppRoleQuery : IRequest<Result<RoleItemDto>>, IdValidationBase
    {
        public Guid? Id { get; set; }
        public GetPassivedByIdAppRoleQuery(Guid? id) => Id = id;
    }
}