using QrAssignment.Domain.Shared.PagePermission;

namespace QrAssignment.Application.Features.Permission.Queries.DTOs;

/// <summary>Rapor ve Excel export'un ortak filtresi. IRequest DEĞİL.</summary>
public abstract class PermissionReportFilterBase
{
    public PermissionOwnerType? OwnerType { get; set; }
    public Guid? UserId { get; set; }
    public Guid? RoleId { get; set; }
    public short? MenuGroupId { get; set; }
    public int? PageId { get; set; }
    public PageAccessFlags? HasFlag { get; set; }
    public bool OnlyGranted { get; set; } = true;
}
