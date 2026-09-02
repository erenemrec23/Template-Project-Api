using MediatR;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.QrLocations.Queries.DTOs;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.QrLocations.Queries.ListBase.GetPassivedList
{
    public class GetPassivedListQrLocationQueryHandler : IRequestHandler<GetPassivedListQrLocationQuery, Result<Paginate<QrLocationListItemDto>>>
    {
        private readonly IQrLocationRepository _qrLocationRepository;

        public GetPassivedListQrLocationQueryHandler(IQrLocationRepository qrLocationRepository)
        {
            _qrLocationRepository = qrLocationRepository;
        }

        public async Task<Result<Paginate<QrLocationListItemDto>>> Handle(GetPassivedListQrLocationQuery request, CancellationToken cancellationToken)
        {
            var result = await _qrLocationRepository.GetPassivedDtoListAsync(request, cancellationToken);

            return Result.Success(result);
        }
    }
}
