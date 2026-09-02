using MediatR;
using QrAssignment.Application.Common.Excel;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.QrLocations.Queries.ListBase.GetListExportExcel
{
    public class GetListQrLocationExportExcelQueryHandler
        : IRequestHandler<GetListQrLocationExportExcelQuery, Result<ExcelFileDto>>
    {
        private const string XlsxContentType =
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        private readonly IQrLocationRepository _qrLocationRepository;
        private readonly IExcelDataExportGenerator _excelGenerator;

        public GetListQrLocationExportExcelQueryHandler(
            IQrLocationRepository qrLocationRepository,
            IExcelDataExportGenerator excelGenerator)
        {
            _qrLocationRepository = qrLocationRepository;
            _excelGenerator = excelGenerator;
        }

        public async Task<Result<ExcelFileDto>> Handle(
            GetListQrLocationExportExcelQuery request, CancellationToken cancellationToken)
        {
            var dataList = await _qrLocationRepository.GetExportListAsync(request, cancellationToken);

            byte[] excelBytes = _excelGenerator.Generate(dataList);

            var resultDto = new ExcelFileDto
            {
                Data = excelBytes,
                FileName = $"QrLokasyonlar_Listesi_{DateTime.Now:yyyyMMdd_HHmm}.xlsx",
                ContentType = XlsxContentType
            };

            return Result.Success(resultDto);
        }
    }
}
