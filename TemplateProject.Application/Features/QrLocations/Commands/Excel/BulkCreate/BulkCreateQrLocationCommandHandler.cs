using AutoMapper;
using MediatR;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.QrLocations.Commands.Excel.BulkCreate
{
    public class BulkCreateQrLocationCommandHandler : IRequestHandler<BulkCreateQrLocationCommand, Result<List<Guid>>>
    {
        private readonly IMapper _mapper;
        private readonly IQrLocationRepository _qrLocationRepository;

        public BulkCreateQrLocationCommandHandler(IQrLocationRepository qrLocationRepository, IMapper mapper)
        {
            _mapper = mapper;
            _qrLocationRepository = qrLocationRepository;
        }

        public async Task<Result<List<Guid>>> Handle(BulkCreateQrLocationCommand request, CancellationToken cancellationToken)
        {
            if (request.Items == null || !request.Items.Any())
            {
                return Result.Failure<List<Guid>>(new Error("Yüklenecek geçerli bir veri bulunamadı.", "QRLOCATION_BULK_CREATE_NO_DATA"));
            }
            var resultIdList = new List<Guid>();
            var codeIsNullList = request.Items.Where(w => !w.Code.HasValue);
            if (codeIsNullList.Any())
            {
                var qrLocationList = _mapper.Map<List<QrLocation>>(codeIsNullList);

                await _qrLocationRepository.AddRangeAsync(qrLocationList, cancellationToken);
                resultIdList.AddRange(qrLocationList.Select(t => t.Id).ToList());
            }
            var codeIsNotNullList = request.Items.Where(w => w.Code.HasValue).Select(s => s.Code.Value).ToList();

            var dataListForUpdate = _qrLocationRepository.GetByRevNumsAsync(codeIsNotNullList, cancellationToken);

            var resultHasNoUpdateData = new List<long>();
            foreach (var code in codeIsNotNullList)
            {
                var dataForUpdate = dataListForUpdate.Result.SingleOrDefault(s => s.RevNum == code);
                var requestDto = request.Items.Single(w => w.Code == code);
                if (dataForUpdate != null)
                {
                    var result = _mapper.Map(requestDto, dataForUpdate);
                    _qrLocationRepository.Update(result);
                    resultIdList.Add(result.Id);
                }
                else
                {
                    resultHasNoUpdateData.Add(code);
                }
            }
            if (resultHasNoUpdateData.Any())
            {
                Result.Failure(new Error("HasNoUpdateData", string.Format("Girmiş Olduğunuz Kod(lar)a Ait Bir Data Bulunamadı ({0})", string.Join(", ", resultHasNoUpdateData))));
            }

            return Result.Success(resultIdList);
        }
    }
}
