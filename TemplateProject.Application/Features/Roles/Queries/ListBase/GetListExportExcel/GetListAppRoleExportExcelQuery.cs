using MediatR;
using QrAssignment.Application.Common.Excel;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Queries.ListBase.GetListExportExcel
{
    public class GetListAppRoleExportExcelQuery : PageRequestBaseDto, IRequest<Result<ExcelFileDto>>
    {
    }
}