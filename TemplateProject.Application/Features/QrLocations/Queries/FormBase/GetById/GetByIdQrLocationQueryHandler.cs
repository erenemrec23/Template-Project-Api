using MediatR;
using QrAssignment.Application.Features.QrLocations.DTOs;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.QrLocations.Queries.FormBase.GetById
{
    public class GetByIdQrLocationQueryHandler : IRequestHandler<GetByIdQrLocationQuery, Result<QrLocationItemDto>>
    {
        private readonly IQrLocationRepository _qrLocationRepository;

        public GetByIdQrLocationQueryHandler(IQrLocationRepository qrLocationRepository)
        {
            _qrLocationRepository = qrLocationRepository;
        }

        public async Task<Result<QrLocationItemDto>> Handle(GetByIdQrLocationQuery request, CancellationToken cancellationToken)
        {
            var result = await _qrLocationRepository.GetDtoByIdAsync(request.Id.Value, cancellationToken);

            return Result.Success(result);
        }
    }
}
