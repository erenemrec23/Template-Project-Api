using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using QrAssignment.Application.Services;

namespace QrAssignment.Infrastructure.Storage
{
    internal sealed class LocalFileStorageService : IFileStorageService
    {
        private readonly LocalStorageOptions _opt;
        private readonly string _root;
        private readonly IHttpContextAccessor _http;

        public LocalFileStorageService(
            IOptions<StorageOptions> opt,
            IHostEnvironment env,
            IHttpContextAccessor http)
        {
            _opt = opt.Value.Local;
            _root = Path.IsPathRooted(_opt.RootPath)
                ? _opt.RootPath
                : Path.Combine(env.ContentRootPath, _opt.RootPath);
            _http = http;
        }

        public async Task<StoredFile> SaveAsync(Stream content, string fileName, string folder,
                                                string? contentType = null, CancellationToken ct = default)
        {
            var name = $"{Guid.NewGuid():N}{Path.GetExtension(fileName)}";
            var key = $"{folder}/{name}";                 // DB'ye yazılan değer

            var dir = Path.Combine(_root, folder);
            Directory.CreateDirectory(dir);

            await using var fs = new FileStream(
                Path.Combine(dir, name), FileMode.Create, FileAccess.Write);
            await content.CopyToAsync(fs, ct);

            return new StoredFile(key, GetUrl(key));
        }

        public Task DeleteAsync(string key, CancellationToken ct = default)
        {
            var path = Path.Combine(_root, key.Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(path)) File.Delete(path);
            return Task.CompletedTask;
        }

        // RootPath "wwwroot/uploads" ise UseStaticFiles ile "/uploads/{key}" servis edilir.
        public string GetUrl(string key)
        {
            if (!string.IsNullOrWhiteSpace(_opt.PublicBaseUrl))
                return $"{_opt.PublicBaseUrl.TrimEnd('/')}/{key}";

            var req = _http.HttpContext?.Request;
            var origin = req is not null ? $"{req.Scheme}://{req.Host}" : "";
            return $"{origin}/uploads/{key}";     // API host'undan servis edilir
        }
    }
}