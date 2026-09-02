using AutoMapper;
using MediatR;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.QrLocations.Commands.SetPassive
{
    public class SetPassiveQrLocationCommandHandler : IRequestHandler<SetPassiveQrLocationCommand, Result>
    {
        private readonly IQrLocationRepository _qrLocationRepository;
        private readonly IMapper _mapper;
        private readonly IAppLocalizer _localizer;

        public SetPassiveQrLocationCommandHandler(IQrLocationRepository qrLocationRepository, IMapper mapper, IAppLocalizer localizer)
        {
            _qrLocationRepository = qrLocationRepository;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result> Handle(SetPassiveQrLocationCommand request, CancellationToken cancellationToken)
        {
            if (!request.Id.HasValue)
                throw new Exception(_localizer["Messages.IdIsNull"]);

            await _qrLocationRepository.SetPassiveByIdAsync(request.Id.Value, cancellationToken);

            return Result.Success();
        }
    }
}
