namespace QrAssignment.Persistance.Options
{
    public sealed class TwoFactorOptions
    {
        public const string SectionName = "TwoFactor";

        // Authenticator uygulamasinda gorunecek isim (canli/local farkli olabilir).
        public string Issuer { get; set; } = "QrAssignment";

        // TOTP parametreleri (standart: 6 hane / 30 sn).
        public int Digits { get; set; } = 6;
        public int Period { get; set; } = 30;

        // Saat kaymasi toleransi: kac onceki/sonraki pencere kabul edilsin.
        public int VerificationWindowPrevious { get; set; } = 1;
        public int VerificationWindowFuture { get; set; } = 1;
    }
}