using MediatR;
using QrAssignment.Application.Common.Excel;
using QrAssignment.Application.Features.Permission.Queries.DTOs;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;
using QrAssignment.Domain.Shared.PagePermission;
using QrAssignment.Application.Features.Permission.Queries.GetPermissionReport;

namespace QrAssignment.Application.Features.Permission.Commands.Excel;

public sealed class ExportPermissionReportExcelQueryHandler
    : IRequestHandler<ExportPermissionReportExcelQuery, Result<ExcelFileDto>>
{
    private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    private readonly ISender _sender;
    private readonly IExcelDataExportGenerator _excelGenerator;
    private readonly IAppLocalizer _l;

    public ExportPermissionReportExcelQueryHandler(ISender sender, IExcelDataExportGenerator excelGenerator, IAppLocalizer l)
    {
        _sender = sender;
        _excelGenerator = excelGenerator;
        _l = l;
    }

    public async Task<Result<ExcelFileDto>> Handle(ExportPermissionReportExcelQuery q, CancellationToken ct)
    {
        var report = await _sender.Send(new GetPermissionReportQuery(q), ct);
        if (report.IsFailure)
            return Result.Failure<ExcelFileDto>(report.Error);

        var rows = report.Value.Select(ToExcelRow).ToList();
        byte[] excelBytes = _excelGenerator.Generate(rows);

        return Result.Success(new ExcelFileDto
        {
            Data = excelBytes,
            FileName = $"{_l["Excel.PermissionReport.FileName"]}_{DateTime.Now:yyyyMMdd_HHmm}.xlsx",
            ContentType = XlsxContentType
        });
    }

    private PermissionReportExcelDto ToExcelRow(PermissionReportItemDto r)
    {
        static string Y(bool b) => b ? "✓" : "";

        return new PermissionReportExcelDto
        {
            OwnerType = _l[r.OwnerType == PermissionOwnerType.User ? "Label.User" : "Label.Role"],
            OwnerName = r.OwnerName,
            MenuGroupName = r.MenuGroupKey is null ? "" : _l[$"Menu.{r.MenuGroupKey}"],
            PageName = _l[$"Page.{r.Key}"],

            View = Y(r.View),
            Insert = Y(r.Insert),
            Update = Y(r.Update),
            Delete = Y(r.Delete),
            SetPassive = Y(r.SetPassive),
            SetActive = Y(r.SetActive),
            ViewPassive = Y(r.ViewPassive),
            ExportExcel = Y(r.ExportExcel),
            ImportExcel = Y(r.ImportExcel),
            ManagePagePermissions = Y(r.ManagePagePermissions),

            Sources = string.Join(", ", r.Sources.Select(SourceLabel)),
            PermissionValue = r.PermissionValue
        };
    }

    private string SourceLabel(PermissionSourceInfo s)
    {
        var group = s.MenuGroupKey is null ? "" : _l[$"Menu.{s.MenuGroupKey}"];
        return s.Kind switch
        {
            "Direct" => _l["Label.SourceDirect"],
            "Group" => $"{_l["Label.SourceGroup"]}: {group}",
            "Role" => $"{_l["Label.SourceRole"]}: {s.RoleName}",
            "RoleGroup" => $"{_l["Label.SourceRoleGroup"]}: {s.RoleName}/{group}",
            _ => s.Kind
        };
    }
}