using MediatR;
using QrAssignment.Application.Features.QrLocations.DTOs;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.QrLocations.Queries.GetById
{
    public class GetByIdQrLocationQueryHandler : IRequestHandler<GetQrLocationByIdQuery, Result<QrLocationItemDto>>
    {
        private readonly IQrLocationRepository _qrLocationRepository;

        public GetByIdQrLocationQueryHandler(IQrLocationRepository qrLocationRepository)
        {
            _qrLocationRepository = qrLocationRepository;
        }

        public async Task<Result<QrLocationItemDto>> Handle(GetQrLocationByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _qrLocationRepository.GetDtoByIdAsync(request.Id, cancellationToken);

            return Result.Success(result);
        }
    }
}
