using AutoMapper;
using MediatR;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.QrLocations.Commands.Delete
{
    public class DeleteQrLocationCommandHandler : IRequestHandler<DeleteQrLocationCommand, Result>
    {
        private readonly IQrLocationRepository _qrLocationRepository;
        private readonly IAppLocalizer _localizer;

        public DeleteQrLocationCommandHandler(IQrLocationRepository qrLocationRepository, IMapper mapper, IAppLocalizer localizer)
        {
            _qrLocationRepository = qrLocationRepository;
            _localizer = localizer;
        }

        public async Task<Result> Handle(DeleteQrLocationCommand request, CancellationToken cancellationToken)
        {
            if (!request.Id.HasValue)
                throw new Exception(_localizer["Messages.IdIsNull"]);

            var qrLocation = await _qrLocationRepository.GetByIdAsync(request.Id.Value, cancellationToken);

            if (qrLocation == null)
                throw new Exception(_localizer["Messages.QrLocationNotFound"]);

            _qrLocationRepository.Delete(qrLocation);
            return Result.Success();
        }
    }
}
