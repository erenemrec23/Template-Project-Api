using AutoMapper;
using MediatR;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.QrLocations.Commands.BulkSetPassive
{
    public class BulkSetPassiveQrLocationCommandHandler : IRequestHandler<BulkSetPassiveQrLocationCommand, Result>
    {
        private readonly IQrLocationRepository _qrLocationRepository;

        public BulkSetPassiveQrLocationCommandHandler(IQrLocationRepository qrLocationRepository, IMapper mapper, IAppLocalizer localizer)
        {
            _qrLocationRepository = qrLocationRepository;
        }

        public async Task<Result> Handle(BulkSetPassiveQrLocationCommand request, CancellationToken cancellationToken)
        {
            await _qrLocationRepository.BulkSetPassiveByIdsAsync(request.IdList, cancellationToken);
            return Result.Success();
        }
    }
}
