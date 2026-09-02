using MediatR;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.QrLocations.Queries.DTOs;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.QrLocations.Queries.GetList
{
    public class GetListQrLocationQueryHandler : IRequestHandler<GetListQrLocationQuery, Result<Paginate<QrLocationListItemDto>>>
    {
        private readonly IQrLocationRepository _qrLocationRepository;

        public GetListQrLocationQueryHandler(IQrLocationRepository qrLocationRepository)
        {
            _qrLocationRepository = qrLocationRepository;
        }

        public async Task<Result<Paginate<QrLocationListItemDto>>> Handle(GetListQrLocationQuery request, CancellationToken cancellationToken)
        {
            var result = await _qrLocationRepository.GetDtoListAsync(request,cancellationToken);

            return Result.Success(result);
        }
    }
}
