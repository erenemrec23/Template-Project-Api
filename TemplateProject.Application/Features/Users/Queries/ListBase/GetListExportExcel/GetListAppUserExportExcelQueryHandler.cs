using MediatR;
using QrAssignment.Application.Common.Excel;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Queries.ListBase.GetListExportExcel
{
    public class GetListAppUserExportExcelQueryHandler
        : IRequestHandler<GetListAppUserExportExcelQuery, Result<ExcelFileDto>>
    {
        private const string XlsxContentType =
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        private readonly IAppUserRepository _appUserRepository;
        private readonly IExcelDataExportGenerator _excelGenerator;

        public GetListAppUserExportExcelQueryHandler(
            IAppUserRepository appUserRepository,
            IExcelDataExportGenerator excelGenerator)
        {
            _appUserRepository = appUserRepository;
            _excelGenerator = excelGenerator;
        }

        public async Task<Result<ExcelFileDto>> Handle(
            GetListAppUserExportExcelQuery request, CancellationToken cancellationToken)
        {
            var dataList = await _appUserRepository.GetExportListAsync(request, cancellationToken);

            byte[] excelBytes = _excelGenerator.Generate(dataList);

            var resultDto = new ExcelFileDto
            {
                Data = excelBytes,
                FileName = $"Kullanicilar_Listesi_{DateTime.Now:yyyyMMdd_HHmm}.xlsx",
                ContentType = XlsxContentType
            };

            return Result.Success(resultDto);
        }
    }
}