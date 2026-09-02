using QrAssignment.Domain.Abstractions;

namespace QrAssignment.Domain.Entity.App
{
    // AppUser ile 1-1 iliskili 2FA yapisi. Identity'nin gomulu authenticator
    // kolonlari yerine bu tabloda tutuluyor.
    public class AppUserTwoFactor : IMustHaveTenant
    {
        // Ayni zamanda PK ve FK (shared primary key => gercek 1-1)
        public Guid AppUserId { get; set; }
        public AppUser AppUser { get; set; } = default!;

        public bool IsEnabled { get; set; }

        // TOTP paylasilan gizli anahtar (Base32). Prod'da IDataProtector ile sifrelenmeli.
        public string? SecretKey { get; set; }

        public DateTimeOffset? EnabledDate { get; set; }

        public Guid? TenantId { get; set; }

        private AppUserTwoFactor() { }

        public static AppUserTwoFactor Create(Guid appUserId) => new() { AppUserId = appUserId, IsEnabled = false };

        public void SetSecret(string base32Secret)
        {
            SecretKey = base32Secret;
            IsEnabled = false;      // yeni secret => tekrar dogrulanana kadar pasif
            EnabledDate = null;
        }

        public void Enable() { IsEnabled = true; EnabledDate = DateTimeOffset.UtcNow; }

        public void Disable() { IsEnabled = false; SecretKey = null; EnabledDate = null; }
    }
}