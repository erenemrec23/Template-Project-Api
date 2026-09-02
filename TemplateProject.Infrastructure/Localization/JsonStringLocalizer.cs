using Microsoft.Extensions.Localization;
using System.Globalization;

namespace QrAssignment.Infrastructure.Localization;

public class JsonStringLocalizer : IStringLocalizer
{
    private readonly JsonLocalizationManager _manager;

    public JsonStringLocalizer(JsonLocalizationManager manager)
    {
        _manager = manager;
    }

    public LocalizedString this[string name]
    {
        get
        {
            // İsteği atan kullanıcının HTTP Header'ındaki dili yakalar (Accept-Language: tr-TR)
            var currentLang = CultureInfo.CurrentCulture.Name;
            var value = _manager.GetValue(currentLang, name);

            // Eğer JSON içinde bulamazsa, key'in kendisini döndürür (Fallback)
            return new LocalizedString(name, value, resourceNotFound: value == name);
        }
    }

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var localizedString = this[name];
            return new LocalizedString(name, string.Format(localizedString.Value, arguments), localizedString.ResourceNotFound);
        }
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) => throw new NotImplementedException();
}
