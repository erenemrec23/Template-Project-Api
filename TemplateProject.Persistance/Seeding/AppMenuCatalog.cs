using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Shared;
using QrAssignment.Domain.Shared.Menu;
using System.Reflection;

namespace QrAssignment.Persistance.Seeding
{
    public static class AppMenuCatalog
    {
        public static List<MenuGroup> BuildGroups() =>
            Enum.GetValues<AppMenuGroup>().Select(g =>
            {
                var a = Field(g).GetCustomAttribute<MenuGroupDefinitionAttribute>();
                return new MenuGroup
                {
                    Id = (short)g,
                    Key = g.ToString(),
                    Icon = a?.Icon ?? string.Empty,
                    Order = a?.Order ?? 0
                };
            }).ToList();

        public static List<Page> BuildPages() =>
            Enum.GetValues<AppPage>().Select(p =>
            {
                var a = Field(p).GetCustomAttribute<PageDefinitionAttribute>()
                    ?? throw new InvalidOperationException($"AppPage.{p} üzerinde [PageDefinition] yok.");
                return new Page
                {
                    Id = (int)p,
                    PageKey = a.PageKey,
                    Key = a.TranslationKey,
                    Icon = a.Icon,
                    Route = string.IsNullOrWhiteSpace(a.Route) ? null : a.Route,
                    Order = a.Order,
                    ShowInMenu = a.ShowInMenu,
                    MenuGroupId = a.Group == 0 ? null : (short)a.Group
                };
            }).ToList();

        private static FieldInfo Field(Enum value) =>
            value.GetType().GetField(value.ToString())!;
    }
}