using MediatR;
using QrAssignment.Application.Common.Excel;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.QrLocations.Queries.ListBase.GetListExportExcel
{
    public class GetListQrLocationExportExcelQuery : PageRequestBaseDto, IRequest<Result<ExcelFileDto>>
    {

    }
}
