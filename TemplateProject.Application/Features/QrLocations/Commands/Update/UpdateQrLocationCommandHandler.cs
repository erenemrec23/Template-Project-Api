using AutoMapper;
using MediatR;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.QrLocations.Commands.Update
{
    public class UpdateQrLocationCommandHandler : IRequestHandler<UpdateQrLocationCommand, Result<UpdateQrLocationResponse>>
    {
        private readonly IQrLocationRepository _qrLocationRepository;
        private readonly IMapper _mapper;
        private readonly IAppLocalizer _localizer;
        public UpdateQrLocationCommandHandler(IQrLocationRepository qrLocationRepository, IMapper mapper, IAppLocalizer localizer)
        {
            _qrLocationRepository = qrLocationRepository;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<UpdateQrLocationResponse>> Handle(UpdateQrLocationCommand request, CancellationToken cancellationToken)
        {
            if (!request.Id.HasValue)
                throw new Exception(_localizer["Messages.IdIsNull"]);
            var qrLocation = await _qrLocationRepository.GetByIdAsync(request.Id.Value, cancellationToken);

            if (qrLocation == null)
                throw new Exception(_localizer["Messages.QrLocationNotFound"]);

            _mapper.Map(request, qrLocation);

            _qrLocationRepository.Update(qrLocation);

            var response = new UpdateQrLocationResponse();
            _mapper.Map(qrLocation, response);

            return Result.Success(response);
        }
    }
}
