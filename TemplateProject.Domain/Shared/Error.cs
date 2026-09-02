namespace QrAssignment.Domain.Shared
{
    public sealed record Error(string Code = "", string Message ="")
    {
        // Hiçbir hata olmayan (Başarılı) durumlar için varsayılan boş bir Error tanımı.
        public static readonly Error None = new(string.Empty, string.Empty);

        // Sık kullanılan genel bir null hatası tanımı.
        public static readonly Error NullValue = new("Error.NullValue", "Beklenen değer null olamaz.");
    }
}
