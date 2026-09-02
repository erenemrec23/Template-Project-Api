using MediatR;
using QrAssignment.Application.Common.Excel;
using QrAssignment.Application.Features.Permission.Queries.DTOs;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Permission.Commands.Excel;

public sealed class ExportPermissionReportExcelQuery : PermissionReportFilterBase, IRequest<Result<ExcelFileDto>> { }
