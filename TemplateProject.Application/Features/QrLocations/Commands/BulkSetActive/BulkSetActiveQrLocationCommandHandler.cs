using AutoMapper;
using MediatR;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.QrLocations.Commands.BulkSetActive
{
    public class BulkSetActiveQrLocationCommandHandler : IRequestHandler<BulkSetActiveQrLocationCommand, Result>
    {
        private readonly IQrLocationRepository _qrLocationRepository;

        public BulkSetActiveQrLocationCommandHandler(IQrLocationRepository qrLocationRepository, IMapper mapper, IAppLocalizer localizer)
        {
            _qrLocationRepository = qrLocationRepository;
        }

        public async Task<Result> Handle(BulkSetActiveQrLocationCommand request, CancellationToken cancellationToken)
        {
            await _qrLocationRepository.BulkSetActiveByIdsAsync(request.IdList, cancellationToken);
            return Result.Success();
        }
    }
}
