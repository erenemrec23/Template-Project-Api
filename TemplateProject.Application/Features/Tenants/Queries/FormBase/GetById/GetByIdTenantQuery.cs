using MediatR;
using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Features.Tenants.DTOs;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Queries.FormBase.GetById
{
    public class GetByIdTenantQuery : IRequest<Result<TenantItemDto>>, IdValidationBase
    {
        public Guid? Id { get; set; }

        public GetByIdTenantQuery(Guid? id)
        {
            Id = id;
        }
    }
}
