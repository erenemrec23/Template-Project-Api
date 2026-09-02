using MediatR;
using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Features.QrLocations.DTOs;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.QrLocations.Queries.FormBase.GetPassivedById
{
    public class GetPassivedByIdQrLocationQuery : IRequest<Result<QrLocationItemDto>>, IdValidationBase
    {
        public Guid? Id { get; set; }

        public GetPassivedByIdQrLocationQuery(Guid? id)
        {
            Id = id;
        }
    }
}
