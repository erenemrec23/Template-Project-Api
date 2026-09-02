using MediatR;
using QrAssignment.Application.Common.Excel;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Queries.ListBase.GetListExportExcel
{
    public class GetListAppUserExportExcelQuery : PageRequestBaseDto, IRequest<Result<ExcelFileDto>>
    {
    }
}