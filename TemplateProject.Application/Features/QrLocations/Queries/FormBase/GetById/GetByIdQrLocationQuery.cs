using MediatR;
using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Features.QrLocations.DTOs;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.QrLocations.Queries.FormBase.GetById
{
    public class GetByIdQrLocationQuery : IRequest<Result<QrLocationItemDto>>, IdValidationBase
    {
        public Guid? Id { get; set; }

        public GetByIdQrLocationQuery(Guid? id)
        {
            Id = id;
        }
    }
}
