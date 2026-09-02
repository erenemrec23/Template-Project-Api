using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OtpNet;
using QrAssignment.Application.Services;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Shared;
using QrAssignment.Persistance.Context;
using QrAssignment.Persistance.Options;

namespace QrAssignment.Persistance.Services
{
    public sealed class TwoFactorService : ITwoFactorService
    {
        // Purpose degistirilirse eski sifreli secret'lar cozulemez -> versiyonlu tuttum.
        private const string ProtectorPurpose = "AppUserTwoFactor.SecretKey.v1";

        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IDataProtector _protector;
        private readonly TwoFactorOptions _options;

        public TwoFactorService(
            AppDbContext context,
            UserManager<AppUser> userManager,
            IDataProtectionProvider dataProtectionProvider,
            IOptions<TwoFactorOptions> options)
        {
            _context = context;
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(ProtectorPurpose);
            _options = options.Value;
        }

        public async Task<bool> IsEnabledAsync(Guid userId, CancellationToken ct = default)
        {
            var tf = await _context.AppUserTwoFactors
            .IgnoreQueryFilters(["TenantFilter"])
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.AppUserId == userId, ct); 
            return tf?.IsEnabled ?? false;
        }

        public async Task<TwoFactorSetupDto> BeginSetupAsync(Guid userId, CancellationToken ct = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString())
                       ?? throw new InvalidOperationException("Kullanici bulunamadi.");

            // Duz Base32 secret (QR icin gerekli), DB'ye SIFRELI yazilacak.
            var base32Secret = Base32Encoding.ToString(KeyGeneration.GenerateRandomKey(20));
            var protectedSecret = _protector.Protect(base32Secret);

            var tf = await _context.Set<AppUserTwoFactor>().FindAsync(new object?[] { userId }, ct);
            if (tf is null)
            {
                tf = AppUserTwoFactor.Create(userId);
                tf.TenantId = user.TenantId;
                tf.SetSecret(protectedSecret);
                await _context.Set<AppUserTwoFactor>().AddAsync(tf, ct);
            }
            else
            {
                tf.SetSecret(protectedSecret); // enable olana kadar pasif
            }

            await _context.SaveChangesAsync(ct);

            var issuer = _options.Issuer;
            var label = Uri.EscapeDataString($"{issuer}:{user.Email}");
            var uri =
                $"otpauth://totp/{label}" +
                $"?secret={base32Secret}" +
                $"&issuer={Uri.EscapeDataString(issuer)}" +
                $"&digits={_options.Digits}" +
                $"&period={_options.Period}";

            // DTO'ya da duz secret gidiyor (manuel giris icin). Istersen bunu da kaldirabilirsin.
            return new TwoFactorSetupDto(base32Secret, uri);
        }
        public async Task<Result> VerifyAndEnableAsync(Guid userId, string code, CancellationToken ct = default)
        {
            var tf = await _context.Set<AppUserTwoFactor>().FindAsync(new object?[] { userId }, ct);
            if (tf?.SecretKey is null)
                return Result.Failure(new Error("TwoFactor.NoSetup", "Once 2FA kurulumu baslatilmali."));

            if (!TryVerify(tf.SecretKey, code))
                return Result.Failure(new Error("TwoFactor.InvalidCode", "Dogrulama kodu hatali."));

            tf.Enable();
            await _context.SaveChangesAsync(ct);
            return Result.Success();
        }

        public async Task<Result> DisableAsync(Guid userId, CancellationToken ct = default)
        {
            var tf = await _context.Set<AppUserTwoFactor>().FindAsync(new object?[] { userId }, ct);
            if (tf is not null)
            {
                tf.Disable();
                await _context.SaveChangesAsync(ct);
            }
            return Result.Success();
        }

        public async Task<bool> VerifyCodeAsync(Guid userId, string code, CancellationToken ct = default)
        {
            var tf = await _context.AppUserTwoFactors
                 .IgnoreQueryFilters(["TenantFilter"])
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.AppUserId == userId, ct);
            if (tf is null || !tf.IsEnabled || tf.SecretKey is null)
                return false;

            return TryVerify(tf.SecretKey, code);
        }

        // VerifyAndEnableAsync icindeki dogrulama mantigini buraya al, ikisi de kullansin.
        private bool TryVerify(string protectedSecret, string code)
        {
            string base32Secret;
            try
            {
                base32Secret = _protector.Unprotect(protectedSecret);
            }
            catch (System.Security.Cryptography.CryptographicException)
            {
                return false;
            }

            var totp = new Totp(
                Base32Encoding.ToBytes(base32Secret),
                step: _options.Period,
                totpSize: _options.Digits);

            var window = new VerificationWindow(
                previous: _options.VerificationWindowPrevious,
                future: _options.VerificationWindowFuture);

            return totp.VerifyTotp(code, out _, window);
        }
    }
}