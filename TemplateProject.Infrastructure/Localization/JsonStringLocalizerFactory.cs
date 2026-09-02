using Microsoft.Extensions.Localization;

namespace QrAssignment.Infrastructure.Localization;

// .NET'in bu Localizer'ı üretebilmesi için Factory sınıfı
public class JsonStringLocalizerFactory : IStringLocalizerFactory
{
    private readonly JsonLocalizationManager _manager;
    public JsonStringLocalizerFactory(JsonLocalizationManager manager) => _manager = manager;

    public IStringLocalizer Create(Type resourceSource) => new JsonStringLocalizer(_manager);
    public IStringLocalizer Create(string baseName, string location) => new JsonStringLocalizer(_manager);
}