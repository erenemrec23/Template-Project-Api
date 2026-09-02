using Microsoft.AspNetCore.Hosting;
using QrAssignment.Application.Interfaces;
using System.Collections.Concurrent;
using System.Text.Json;

namespace QrAssignment.Infrastructure.Localization;

public class JsonLocalizationManager :   ILocalizationService
{
    private readonly string _folderPath;
    private readonly SemaphoreSlim _fileLock = new(1, 1);

    // Her dilin sözlüğünü RAM'de tuttuğumuz Cache (Sıfır I/O, anında yanıt)
    private readonly ConcurrentDictionary<string, Dictionary<string, string>> _cache = new();

    public JsonLocalizationManager(IWebHostEnvironment env)
    {
        // JSON dosyalarının duracağı klasör (wwwroot/i18n vb.)
        _folderPath = Path.Combine(env.ContentRootPath, "LocalizationFiles");
        if (!Directory.Exists(_folderPath)) Directory.CreateDirectory(_folderPath);
    }

    // Backend (IStringLocalizer) bir kelime aradığında buraya gelir
    public string GetValue(string languageCode, string key)
    {
        if (!_cache.ContainsKey(languageCode))
        {
            LoadLanguageToCache(languageCode);
        }

        return _cache[languageCode].TryGetValue(key, out var value) ? value : key;
    }

    // Angular uygulaması ayağa kalkarken tüm sözlüğü tek seferde çekmek için
    public Dictionary<string, string> GetAll(string languageCode)
    {
        if (!_cache.ContainsKey(languageCode)) LoadLanguageToCache(languageCode);
        return _cache[languageCode];
    }

    // Admin panelden bir çeviri güncellendiğinde çağrılır
    public async Task UpdateKeyAsync(string languageCode, string key, string newValue)
    {
        await _fileLock.WaitAsync();
        try
        {
            if (!_cache.ContainsKey(languageCode)) LoadLanguageToCache(languageCode);

            // 1. RAM'i Güncelle
            _cache[languageCode][key] = newValue;

            // 2. Fiziksel Dosyayı Güncelle
            var filePath = Path.Combine(_folderPath, $"{languageCode}.json");
            var jsonContent = JsonSerializer.Serialize(_cache[languageCode], new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(filePath, jsonContent);
        }
        finally
        {
            _fileLock.Release();
        }
    }

    private void LoadLanguageToCache(string languageCode)
    {
        var filePath = Path.Combine(_folderPath, $"{languageCode}.json");
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new();
            _cache[languageCode] = dict;
        }
        else
        {
            _cache[languageCode] = new Dictionary<string, string>();
        }
    }
}