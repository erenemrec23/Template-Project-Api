using MediatR;
using QrAssignment.Application.Common.Excel;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Queries.ListBase.GetListExportExcel
{
    public class GetListTenantExportExcelQuery : PageRequestBaseDto, IRequest<Result<ExcelFileDto>>
    {

    }

     
}
