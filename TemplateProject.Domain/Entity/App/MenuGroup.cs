namespace QrAssignment.Domain.Entity.App
{
    public sealed class MenuGroup
    {
        public short Id { get; set; }                       // = AppMenuGroup değeri
        public string Key { get; set; } = string.Empty;     // "Admin" (çeviri anahtarı)
        public string Icon { get; set; } = string.Empty;
        public int Order { get; set; }

        public ICollection<Page> Pages { get; set; } = new List<Page>();
    }

    public sealed class Page
    {
        public int Id { get; set; }                         // = AppPage değeri (stabil)
        public string PageKey { get; set; } = string.Empty; // "Page_Users"
        public string Key { get; set; } = string.Empty;     // "Users" (çeviri anahtarı)
        public string Icon { get; set; } = string.Empty;
        public string? Route { get; set; }
        public int Order { get; set; }
        public bool ShowInMenu { get; set; } = true;

        public short? MenuGroupId { get; set; }
        public MenuGroup? MenuGroup { get; set; }
    }
}