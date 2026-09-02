using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Services
{
    public sealed record TwoFactorSetupDto(string SecretKey, string AuthenticatorUri);

    public interface ITwoFactorService
    {
        Task<bool> IsEnabledAsync(Guid userId, CancellationToken ct = default);
        Task<TwoFactorSetupDto> BeginSetupAsync(Guid userId, CancellationToken ct = default);
        Task<Result> VerifyAndEnableAsync(Guid userId, string code, CancellationToken ct = default);
        Task<Result> DisableAsync(Guid userId, CancellationToken ct = default); 
        Task<bool> VerifyCodeAsync(Guid userId, string code, CancellationToken ct = default);
    }
}