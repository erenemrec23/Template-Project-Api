using MediatR;
using QrAssignment.Application.Features.Permission.Queries.DTOs;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Permission.Queries.GetPermissionReport;

public sealed class GetPermissionReportQuery : PermissionReportFilterBase, IRequest<Result<List<PermissionReportItemDto>>>
{
    public GetPermissionReportQuery() { }

    public GetPermissionReportQuery(PermissionReportFilterBase f)
    {
        OwnerType = f.OwnerType; UserId = f.UserId; RoleId = f.RoleId;
        MenuGroupId = f.MenuGroupId; PageId = f.PageId; HasFlag = f.HasFlag; OnlyGranted = f.OnlyGranted;
    }
}