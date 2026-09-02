using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QrAssignment.Application.Services;   // IFileStorageService

namespace QrAssignment.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public sealed class FilesController : ControllerBase
    {
        private readonly IFileStorageService _fileStorage;

        // Basit güvenlik ağı — istersen options'a taşı
        private static readonly string[] AllowedExtensions =
            { ".jpg", ".jpeg", ".png", ".webp", ".gif", ".pdf" };
        private const long MaxBytes = 10 * 1024 * 1024; // 10 MB

        // Yalnızca bilinen klasörlere yazsın (path traversal & çöp klasör engeli)
        private static readonly HashSet<string> AllowedFolders =
            new(StringComparer.OrdinalIgnoreCase) { "feedbacks", "avatars", "qr-locations", "misc" };

        public FilesController(IFileStorageService fileStorage) => _fileStorage = fileStorage;

        [HttpPost("Upload")]
        [RequestSizeLimit(MaxBytes)]
        public async Task<IActionResult> Upload(
            IFormFile file,
            [FromQuery] string folder = "misc",
            CancellationToken ct = default)
        {
            if (file is null || file.Length == 0)
                return BadRequest("Dosya boş.");

            if (file.Length > MaxBytes)
                return BadRequest($"Dosya boyutu {MaxBytes / (1024 * 1024)} MB sınırını aşıyor.");

            var ext = Path.GetExtension(file.FileName);
            if (string.IsNullOrWhiteSpace(ext) || !AllowedExtensions.Contains(ext.ToLowerInvariant()))
                return BadRequest("İzin verilmeyen dosya türü.");

            if (!AllowedFolders.Contains(folder))
                folder = "misc";

            await using var stream = file.OpenReadStream();
            var stored = await _fileStorage.SaveAsync(stream, file.FileName, folder, file.ContentType, ct);

            // stored.Key → çağıran taraf DB'ye yazar; stored.Url → önizleme için
            return Ok(stored);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromQuery] string key, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(key))
                return BadRequest("key gerekli.");

            await _fileStorage.DeleteAsync(key, ct);
            return NoContent();
        }
    }
}