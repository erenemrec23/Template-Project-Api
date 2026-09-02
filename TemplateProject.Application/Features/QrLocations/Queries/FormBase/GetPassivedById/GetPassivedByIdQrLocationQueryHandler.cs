using MediatR;
using QrAssignment.Application.Features.QrLocations.DTOs;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.QrLocations.Queries.FormBase.GetPassivedById
{
    public class GetPassivedByIdQrLocationQueryHandler : IRequestHandler<GetPassivedByIdQrLocationQuery, Result<QrLocationItemDto>>
    {
        private readonly IQrLocationRepository _qrLocationRepository;

        public GetPassivedByIdQrLocationQueryHandler(IQrLocationRepository qrLocationRepository)
        {
            _qrLocationRepository = qrLocationRepository;
        }

        public async Task<Result<QrLocationItemDto>> Handle(GetPassivedByIdQrLocationQuery request, CancellationToken cancellationToken)
        {
            var result = await _qrLocationRepository.GetPassivedDtoByIdAsync(request.Id.Value, cancellationToken);

            return Result.Success(result);
        }
    }
}
