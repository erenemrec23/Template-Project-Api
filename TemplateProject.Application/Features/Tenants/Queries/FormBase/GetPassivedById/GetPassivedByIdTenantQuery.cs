using MediatR;
using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Features.Tenants.DTOs;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Queries.FormBase.GetPassivedById
{
    public class GetPassivedByIdTenantQuery : IRequest<Result<TenantItemDto>>, IdValidationBase
    {
        public Guid? Id { get; set; }

        public GetPassivedByIdTenantQuery(Guid? id)
        {
            Id = id;
        }
    }
}
