using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using QrAssignment.Application.Services;

namespace QrAssignment.Infrastructure.Storage
{
    internal sealed class AzureBlobStorageService : IFileStorageService
    {
        private readonly AzureBlobOptions _opt;
        private readonly BlobContainerClient _container;

        public AzureBlobStorageService(IOptions<StorageOptions> opt)
        {
            _opt = opt.Value.AzureBlob;
            _container = new BlobServiceClient(_opt.ConnectionString)
                .GetBlobContainerClient(_opt.Container);
        }

        public async Task<StoredFile> SaveAsync(Stream content, string fileName, string folder,
                                                string? contentType = null, CancellationToken ct = default)
        {
            await _container.CreateIfNotExistsAsync(PublicAccessType.Blob, cancellationToken: ct);

            var key = $"{folder}/{Guid.NewGuid():N}{Path.GetExtension(fileName)}";
            var blob = _container.GetBlobClient(key);

            await blob.UploadAsync(content, new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders { ContentType = contentType }
            }, ct);

            return new StoredFile(key, GetUrl(key));
        }

        public async Task DeleteAsync(string key, CancellationToken ct = default)
            => await _container.GetBlobClient(key).DeleteIfExistsAsync(cancellationToken: ct);

        public string GetUrl(string key)
            => string.IsNullOrWhiteSpace(_opt.PublicBaseUrl)
                ? _container.GetBlobClient(key).Uri.ToString()
                : $"{_opt.PublicBaseUrl.TrimEnd('/')}/{key}";
    }
}