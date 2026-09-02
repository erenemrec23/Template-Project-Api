 
using QrAssignment.Application.Abstractions;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.AuthFeatures.Commands.DisableUserTwoFactor
{
    // Admin, bir kullanicinin 2FA'sini kapatir (ornegin authenticator kaybi).
    // Enable YOK: admin baskasinin secret'ini goremez, sadece disable eder.
    public sealed record DisableUserTwoFactorCommand(Guid UserId) : ICommand<Result>;
}