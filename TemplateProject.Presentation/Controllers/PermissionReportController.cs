using MediatR;
using Microsoft.AspNetCore.Mvc;
using QrAssignment.Application.Features.Permission.Commands.Excel;
using QrAssignment.Application.Features.Permission.Queries.GetPermissionReport;
using QrAssignment.Application.Features.Permission.Queries.GetPermissionReportLookup;

namespace QrAssignment.Presentation.Controllers;

[ApiController]
[Route("api/permission-report")]
public sealed class PermissionReportController : ControllerBase
{
    private readonly ISender _sender;
    public PermissionReportController(ISender sender) => _sender = sender;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetPermissionReportQuery query, CancellationToken ct)
        => Ok(await _sender.Send(query, ct));

    [HttpGet("lookups")]
    public async Task<IActionResult> Lookups(CancellationToken ct)
        => Ok(await _sender.Send(new GetPermissionReportLookupQuery(), ct));

    [HttpGet("export-excel")]
    public async Task<IActionResult> ExportExcel([FromQuery] ExportPermissionReportExcelQuery query, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(query, cancellationToken);
        if (!result.IsSuccess || result.Value is null)
            return BadRequest(result);

        var file = result.Value;
        return File(file.Data, file.ContentType, file.FileName);
    }
}