 
namespace QrAssignment.Domain.Shared.Menu
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class MenuGroupDefinitionAttribute : Attribute
    {
        public string Icon { get; init; } = string.Empty;
        public int Order { get; init; }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public sealed class PageDefinitionAttribute : Attribute
    {
        public AppMenuGroup Group { get; init; }        // atanmazsa 0 → menüsüz sayfa
        public string PageKey { get; init; } = string.Empty;
        public string TranslationKey { get; init; } = string.Empty;
        public string Icon { get; init; } = string.Empty;
        public string Route { get; init; } = string.Empty;
        public int Order { get; init; }
        public bool ShowInMenu { get; init; } = true;
    }
}