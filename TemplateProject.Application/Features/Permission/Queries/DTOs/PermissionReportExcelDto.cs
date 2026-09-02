using QrAssignment.Application.Common.Excel;

namespace QrAssignment.Application.Features.Permission.Queries.DTOs;

public sealed class PermissionReportExcelDto
{
    [ExcelColumn("Excel.PermissionReport.OwnerType", Order = 1)] 
    public string OwnerType { get; init; } = default!;
    [ExcelColumn("Excel.PermissionReport.OwnerName",  Order = 2)] public string OwnerName { get; init; } = default!;
    [ExcelColumn("Excel.PermissionReport.MenuGroup",  Order = 3)] public string MenuGroupName { get; init; } = default!;
    [ExcelColumn("Excel.PermissionReport.Page",  Order = 4)] public string PageName { get; init; } = default!;
    [ExcelColumn("Excel.PermissionReport.View",  Order = 5)] public string View { get; init; } = default!;
    [ExcelColumn("Excel.PermissionReport.Insert",  Order = 6)] public string Insert { get; init; } = default!;
    [ExcelColumn("Excel.PermissionReport.Update",  Order = 7)] public string Update { get; init; } = default!;
    [ExcelColumn("Excel.PermissionReport.Delete",  Order = 8)] public string Delete { get; init; } = default!;
    [ExcelColumn("Excel.PermissionReport.SetPassive",  Order = 9)] public string SetPassive { get; init; } = default!;
    [ExcelColumn("Excel.PermissionReport.SetActive",  Order = 10)] public string SetActive { get; init; } = default!;
    [ExcelColumn("Excel.PermissionReport.ViewPassive",  Order = 11)] public string ViewPassive { get; init; } = default!;
    [ExcelColumn("Excel.PermissionReport.ExportExcel",  Order = 12)] public string ExportExcel { get; init; } = default!;
    [ExcelColumn("Excel.PermissionReport.ImportExcel",  Order = 13)] public string ImportExcel { get; init; } = default!;
    [ExcelColumn("Excel.PermissionReport.ManagePermissions",  Order = 14)] public string ManagePagePermissions { get; init; } = default!;
    [ExcelColumn("Excel.PermissionReport.Sources",  Order = 15)] public string Sources { get; init; } = default!;
    [ExcelColumn("Excel.PermissionReport.Value",  Order = 16)] public int PermissionValue { get; init; }
}
