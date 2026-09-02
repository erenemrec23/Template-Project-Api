namespace QrAssignment.Infrastructure.Storage
{
    public sealed class StorageOptions
    {
        public const string SectionName = "Storage";
        public string Provider { get; set; } = "Local";      // "Local" | "AzureBlob"
        public LocalStorageOptions Local { get; set; } = new();
        public AzureBlobOptions AzureBlob { get; set; } = new();
    }

    public sealed class LocalStorageOptions
    {
        public string RootPath { get; set; } = "wwwroot/uploads"; // ContentRoot'a göre
        public string PublicBaseUrl { get; set; } = "";           // boşsa "/uploads/..."
    }

    public sealed class AzureBlobOptions
    {
        public string ConnectionString { get; set; } = "";       // Azure'da env'den
        public string Container { get; set; } = "uploads";
        public string PublicBaseUrl { get; set; } = "";           // boşsa blob Uri
    }
}