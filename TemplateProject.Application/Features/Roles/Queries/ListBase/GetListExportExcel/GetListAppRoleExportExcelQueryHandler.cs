using MediatR;
using QrAssignment.Application.Common.Excel;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Queries.ListBase.GetListExportExcel
{
    public class GetListAppRoleExportExcelQueryHandler
        : IRequestHandler<GetListAppRoleExportExcelQuery, Result<ExcelFileDto>>
    {
        private const string XlsxContentType =
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        private readonly IAppRoleRepository _appRoleRepository;
        private readonly IExcelDataExportGenerator _excelGenerator;

        public GetListAppRoleExportExcelQueryHandler(
            IAppRoleRepository appRoleRepository,
            IExcelDataExportGenerator excelGenerator)
        {
            _appRoleRepository = appRoleRepository;
            _excelGenerator = excelGenerator;
        }

        public async Task<Result<ExcelFileDto>> Handle(
            GetListAppRoleExportExcelQuery request, CancellationToken cancellationToken)
        {
            var dataList = await _appRoleRepository.GetExportListAsync(request, cancellationToken);

            byte[] excelBytes = _excelGenerator.Generate(dataList);

            var resultDto = new ExcelFileDto
            {
                Data = excelBytes,
                FileName = $"Roller_Listesi_{DateTime.Now:yyyyMMdd_HHmm}.xlsx",
                ContentType = XlsxContentType
            };

            return Result.Success(resultDto);
        }
    }
}