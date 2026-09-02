namespace QrAssignment.Application.Services
{
    public interface IFileStorageService
    {
        // key = "folder/dosya.ext" (DB'ye bu yazılır). Url provider'a göre üretilir.
        Task<StoredFile> SaveAsync(Stream content, string fileName, string folder,
                                   string? contentType = null, CancellationToken ct = default);
        Task DeleteAsync(string key, CancellationToken ct = default);
        string GetUrl(string key);

        string? ResolveUrl(string? keyOrUrl)
            => string.IsNullOrWhiteSpace(keyOrUrl) ? null
             : keyOrUrl.StartsWith("data:") || keyOrUrl.StartsWith("http") ? keyOrUrl
             : GetUrl(keyOrUrl);

    }

    public sealed record StoredFile(string Key, string Url);

}