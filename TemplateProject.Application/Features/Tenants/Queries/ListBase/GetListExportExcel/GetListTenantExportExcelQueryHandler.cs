using MediatR;
using QrAssignment.Application.Common.Excel;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Queries.ListBase.GetListExportExcel
{
    public class GetListTenantExportExcelQueryHandler
        : IRequestHandler<GetListTenantExportExcelQuery, Result<ExcelFileDto>>
    {
        private const string XlsxContentType =
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        private readonly ITenantRepository _tenantRepository;
        private readonly IExcelDataExportGenerator _excelGenerator;

        public GetListTenantExportExcelQueryHandler(
            ITenantRepository tenantRepository,
            IExcelDataExportGenerator excelGenerator)
        {
            _tenantRepository = tenantRepository;
            _excelGenerator = excelGenerator;
        }

        public async Task<Result<ExcelFileDto>> Handle(
            GetListTenantExportExcelQuery request, CancellationToken cancellationToken)
        {
            var dataList = await _tenantRepository.GetExportListAsync(request, cancellationToken);

            byte[] excelBytes = _excelGenerator.Generate(dataList);

            var resultDto = new ExcelFileDto
            {
                Data = excelBytes,
                FileName = $"Firmalar_Listesi_{DateTime.Now:yyyyMMdd_HHmm}.xlsx",
                ContentType = XlsxContentType
            };

            return Result.Success(resultDto);
        }
    }
}