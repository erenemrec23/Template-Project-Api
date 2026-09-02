using AutoMapper;
using MediatR;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.QrLocations.Commands.BulkDelete
{
    public class BulkDeleteQrLocationCommandHandler : IRequestHandler<BulkDeleteQrLocationCommand, Result>
    {
        private readonly IQrLocationRepository _qrLocationRepository;

        public BulkDeleteQrLocationCommandHandler(IQrLocationRepository qrLocationRepository, IMapper mapper, IAppLocalizer localizer)
        {
            _qrLocationRepository = qrLocationRepository;
        }

        public async Task<Result> Handle(BulkDeleteQrLocationCommand request, CancellationToken cancellationToken)
        {
            await _qrLocationRepository.DeleteRange(request.IdList, cancellationToken);
            return Result.Success();
        }
    }
}
