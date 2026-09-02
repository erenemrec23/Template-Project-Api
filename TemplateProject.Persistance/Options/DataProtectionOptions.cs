namespace QrAssignment.Persistance.Options
{
    public sealed class DataProtectionOptions
    {
        public const string SectionName = "DataProtection";
        public string ApplicationName { get; set; } = "QrAssignment";
        public string? KeysPath { get; set; }

        public CertificateOptions Certificate { get; set; } = new();
        public AzureOptions Azure { get; set; } = new();

        public sealed class CertificateOptions
        {
            public string Source { get; set; } = "None";
            public string? Thumbprint { get; set; }
            public string StoreName { get; set; } = "My";
            public string StoreLocation { get; set; } = "CurrentUser";
            public string? FilePath { get; set; }
            public string? Password { get; set; }
        }

        public sealed class AzureOptions
        {
            public string BlobSasOrUri { get; set; } = "";
            public string KeyVaultKeyId { get; set; } = "";
        }
    }
}