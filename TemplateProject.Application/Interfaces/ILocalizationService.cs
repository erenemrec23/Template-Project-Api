namespace QrAssignment.Application.Interfaces;

public interface ILocalizationService
{
    string GetValue(string languageCode, string key);
    Dictionary<string, string> GetAll(string languageCode);
    Task UpdateKeyAsync(string languageCode, string key, string newValue);
}