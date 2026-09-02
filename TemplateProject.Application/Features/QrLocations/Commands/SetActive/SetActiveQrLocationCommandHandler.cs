using AutoMapper;
using MediatR;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.QrLocations.Commands.SetActive
{
    public class SetActiveQrLocationCommandHandler : IRequestHandler<SetActiveQrLocationCommand, Result>
    {
        private readonly IQrLocationRepository _qrLocationRepository;
        private readonly IMapper _mapper;
        private readonly IAppLocalizer _localizer;

        public SetActiveQrLocationCommandHandler(IQrLocationRepository qrLocationRepository, IMapper mapper, IAppLocalizer localizer)
        {
            _qrLocationRepository = qrLocationRepository;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result> Handle(SetActiveQrLocationCommand request, CancellationToken cancellationToken)
        {
            if (!request.Id.HasValue)
                throw new Exception(_localizer["Messages.IdIsNull"]);

            var qrLocation = await _qrLocationRepository.GetPassivedByIdAsync(request.Id.Value, cancellationToken);

            if (qrLocation == null)
                throw new Exception(_localizer["Messages.QrLocationNotFound"]);

            qrLocation.IsPassived = false;

            _qrLocationRepository.Update(qrLocation);

            return Result.Success();
        }
    }
}
