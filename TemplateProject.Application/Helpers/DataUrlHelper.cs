namespace QrAssignment.Application.Helpers
{
    public static class DataUrlHelper
    {
        public static (byte[] Bytes, string ContentType, string Extension) Parse(string dataUrl)
        {
            if (string.IsNullOrWhiteSpace(dataUrl))
                throw new ArgumentException("Boş screenshot verisi.");

            var comma = dataUrl.IndexOf(',');
            string base64;
            var contentType = "application/octet-stream";

            if (dataUrl.StartsWith("data:", StringComparison.OrdinalIgnoreCase) && comma > 0)
            {
                var meta = dataUrl[5..comma];             // "image/jpeg;base64"
                var semi = meta.IndexOf(';');
                contentType = semi > 0 ? meta[..semi] : meta;
                base64 = dataUrl[(comma + 1)..];
            }
            else base64 = dataUrl;                        // düz base64 gelirse

            var ext = contentType switch
            {
                "image/jpeg" => ".jpg",
                "image/png" => ".png",
                "image/webp" => ".webp",
                _ => ".bin"
            };
            return (Convert.FromBase64String(base64), contentType, ext);
        }
    }
}