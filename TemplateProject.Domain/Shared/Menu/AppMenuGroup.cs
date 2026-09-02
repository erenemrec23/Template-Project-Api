namespace QrAssignment.Domain.Shared.Menu
{
    public enum AppMenuGroup : short
    {
        [MenuGroupDefinition(Icon = "bi-gear-wide-connected", Order = 1)]
        Admin = 1,
        [MenuGroupDefinition(Icon = "bi-file-earmark-bar-graph", Order = 2)]
        Report = 2,
    }

    public enum AppPage
    {
        [PageDefinition(Group = AppMenuGroup.Admin, PageKey = "Users",
            TranslationKey = "Users", Icon = "bi-people", Route = "/users", Order = 1)]
        Users = 1,

        [PageDefinition(Group = AppMenuGroup.Admin, PageKey = "QrLocations",
            TranslationKey = "QrLocations", Icon = "bi-qr-code", Route = "/qr-locations", Order = 2)]
        QrLocations = 2,

        [PageDefinition(Group = AppMenuGroup.Admin, PageKey = "Tenants",
            TranslationKey = "Tenants", Icon = "bi-shop", Route = "/tenants", Order = 3)]
        Tenants = 3,

        [PageDefinition(Group = AppMenuGroup.Admin, PageKey = "Roles",
            TranslationKey = "AppRoles", Icon = "bi-shield", Route = "/roles", Order = 4)]
        Roles = 4,


        [PageDefinition(Group = AppMenuGroup.Admin, PageKey = "FeedBacks",
    TranslationKey = "FeedBacks", Icon = "bi-chat-right-text-fill", Route = "/feedbacks", Order = 5)]
        FeedBacks = 5,

        // Menüde görünmeyen, sadece yetki kapsamı olan sayfa (AuthorizationRegistry'deki
        // Page_UserPermissions). Grup atanmadı → MenuGroupId null, ShowInMenu false.
        //[PageDefinition(PageKey = "Page_UserPermissions",
        //    TranslationKey = "UserPermissions", Order = 99, ShowInMenu = false)]
        //UserPermissions = 5,


        [PageDefinition(Group = AppMenuGroup.Report, PageKey = "PermissionReports",
    TranslationKey = "PermissionReports", Icon = "bi-shield-lock", Route = "/permission-reports", Order = 6)]
        PermissionReports = 6,
    }
}