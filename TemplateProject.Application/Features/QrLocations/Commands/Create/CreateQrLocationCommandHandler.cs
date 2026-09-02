using AutoMapper;
using MediatR;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.QrLocations.Commands.Create
{
    public class CreateQrLocationCommandHandler : IRequestHandler<CreateQrLocationCommand, Result<Guid>>
    {
        private readonly IMapper _mapper;
        private readonly IQrLocationRepository _qrLocationRepository;
        public CreateQrLocationCommandHandler(IQrLocationRepository qrLocationRepository, IMapper mapper)
        {
            _mapper = mapper;
            _qrLocationRepository = qrLocationRepository;
        }

        public async Task<Result<Guid>> Handle(CreateQrLocationCommand request, CancellationToken cancellationToken)
        {
            var qrLocation = _mapper.Map<QrLocation>(request);
            await _qrLocationRepository.AddAsync(qrLocation, cancellationToken);
            return Result.Success(qrLocation.Id);
        }
    }
}
