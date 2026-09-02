namespace QrAssignment.Domain.Shared.PagePermission
{
    [Flags] // Bu attribute çok kritiktir!
    public enum PageAccessFlags : int
    {
        None = 0,
        View = 1,          
        Insert = 2,         
        Update = 4,
        SetPassive = 8,
        ViewPassive = 16,
        SetActive = 32,
        Delete = 64,
        ExportExcel = 128,  
        ImportExcel = 256,
        ManagePagePermissions = 512,

        // Sık kullanılan kombinasyonlar (Opsiyonel)
        ViewAndInsert = View | Insert,  
        All = View | Insert | Update | SetPassive | ViewPassive | SetActive | Delete |  ExportExcel | ImportExcel | ManagePagePermissions
    }


    public enum PermissionOwnerType : byte { User = 1, Role = 2 }
    public enum PermissionTargetType : byte { Page = 1, MenuGroup = 2 }
    public enum PermissionChangeAction : byte { Added = 1, Updated = 2, Removed = 3 }


}
