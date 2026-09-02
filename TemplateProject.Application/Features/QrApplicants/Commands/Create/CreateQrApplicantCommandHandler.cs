//using AutoMapper;
//using MediatR;
//using QrAssignment.Application.Repositories;
//using QrAssignment.Domain.Entity;
//using QrAssignment.Domain.Shared;

//namespace QrAssignment.Application.Features.QrApplicants.Commands.CreateQrAssignment;

//public class CreateQrApplicantCommandHandler : IRequestHandler<CreateQrApplicantCommand, Result<Guid>>
//{
//    private readonly IMapper _mapper;
//    private readonly IQrApplicantRepository _qrApplicantRepository;
//    public CreateQrApplicantCommandHandler(IQrApplicantRepository qrApplicantRepository, IMapper mapper)
//    {
//        _mapper = mapper;
//        _qrApplicantRepository = qrApplicantRepository;
//    }

//    public async Task<Result<Guid>> Handle(CreateQrApplicantCommand request, CancellationToken cancellationToken)
//    {

//        var qrApplicant = _mapper.Map<QrApplicant>(request);
//        await _qrApplicantRepository.AddAsync(qrApplicant, cancellationToken);
//        return Result.Success(qrApplicant.Id);
//    }
//}