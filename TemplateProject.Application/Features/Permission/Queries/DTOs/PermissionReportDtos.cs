using QrAssignment.Domain.Shared.PagePermission;

namespace QrAssignment.Application.Features.Permission.Queries.DTOs;

// --- Repository'den gelen ham satırlar ---
//public sealed record PermissionSourceRow(
//    PermissionOwnerType OwnerType, Guid OwnerId, string OwnerName,
//    int? PageId, short? MenuGroupId, PageAccessFlags Value);



public sealed record LookupItem<T>(T Id, string Name);

//public sealed record PermissionPageRow(
//    int PageId, string PageKey, string Key, short? MenuGroupId, string? MenuGroupKey, bool ShowInMenu);

public sealed class PermissionReportItemDto
{ 
    public int PageId { get; init; }
    public string PageKey { get; init; } = default!;  
    public string Key { get; init; } = default!;     
    public short? MenuGroupId { get; init; }
    public string? MenuGroupKey { get; init; }     
    public PermissionOwnerType OwnerType { get; init; }
    public Guid OwnerId { get; init; }
    public string OwnerName { get; init; } = default!; 
    public string PageName { get; init; } = default!;
    public string? GroupKey { get; init; } 
    public string MenuGroupName { get; init; } = default!;

    public int PermissionValue { get; init; }
    /// <summary>Örn: "Doğrudan", "Grup: Tanımlar", "Rol: Admin", "Rol Grubu: Admin/Tanımlar"</summary>
    public List<PermissionSourceInfo> Sources { get; init; } = [];

    public bool View => Has(PageAccessFlags.View);
    public bool Insert => Has(PageAccessFlags.Insert);
    public bool Update => Has(PageAccessFlags.Update);
    public bool Delete => Has(PageAccessFlags.Delete);
    public bool SetPassive => Has(PageAccessFlags.SetPassive);
    public bool SetActive => Has(PageAccessFlags.SetActive);
    public bool ViewPassive => Has(PageAccessFlags.ViewPassive);
    public bool ExportExcel => Has(PageAccessFlags.ExportExcel);
    public bool ImportExcel => Has(PageAccessFlags.ImportExcel);
    public bool ManagePagePermissions => Has(PageAccessFlags.ManagePagePermissions);

    private bool Has(PageAccessFlags f) => ((PageAccessFlags)PermissionValue & f) == f;

}

public sealed class PermissionReportLookupDto
{
    public List<LookupItem<Guid>> Users { get; init; } = [];
    public List<LookupItem<Guid>> Roles { get; init; } = [];
    public List<LookupItem<int>> MenuGroups { get; init; } = [];   // Name = Key (çeviri anahtarı)
    public List<PermissionPageRow> Pages { get; init; } = [];
}


public sealed record PermissionSourceRow(PermissionOwnerType OwnerType, Guid OwnerId, string OwnerName, int? PageId, short? MenuGroupId, PageAccessFlags Value);
public sealed record PermissionPageRow(int PageId, string PageKey, string Key, short? MenuGroupId, string? MenuGroupKey, bool ShowInMenu);

public sealed record PermissionSourceInfo(string Kind, string? RoleName, string? MenuGroupKey);