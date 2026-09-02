using MediatR;
using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Services;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Profile.Commands.TwoFactor
{
    // 1) Kurulum baslat -> secret + otpauth uri doner (henuz enable degil)
    public sealed record BeginTwoFactorSetupCommand() : ICommand<Result<TwoFactorSetupDto>>;

    internal sealed class BeginTwoFactorSetupCommandHandler : IRequestHandler<BeginTwoFactorSetupCommand, Result<TwoFactorSetupDto>>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly ITwoFactorService _twoFactor;
        public BeginTwoFactorSetupCommandHandler(ICurrentUserService c, ITwoFactorService t) { _currentUser = c; _twoFactor = t; }

        public async Task<Result<TwoFactorSetupDto>> Handle(BeginTwoFactorSetupCommand request, CancellationToken ct)
            => Result.Success(await _twoFactor.BeginSetupAsync(_currentUser.UserId.Value, ct));
    }

    // 2) Authenticator kodunu dogrula ve aktif et
    public sealed record EnableTwoFactorCommand(string Code) : ICommand<Result>;

    internal sealed class EnableTwoFactorCommandHandler : IRequestHandler<EnableTwoFactorCommand, Result>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly ITwoFactorService _twoFactor;
        public EnableTwoFactorCommandHandler(ICurrentUserService c, ITwoFactorService t) { _currentUser = c; _twoFactor = t; }

        public Task<Result> Handle(EnableTwoFactorCommand request, CancellationToken ct)
            => _twoFactor.VerifyAndEnableAsync(_currentUser.UserId.Value, request.Code, ct);
    }

    // 3) Pasif et
    public sealed record DisableTwoFactorCommand() : ICommand<Result>;

    internal sealed class DisableTwoFactorCommandHandler : IRequestHandler<DisableTwoFactorCommand, Result>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly ITwoFactorService _twoFactor;
        public DisableTwoFactorCommandHandler(ICurrentUserService c, ITwoFactorService t) { _currentUser = c; _twoFactor = t; }

        public Task<Result> Handle(DisableTwoFactorCommand request, CancellationToken ct)
            => _twoFactor.DisableAsync(_currentUser.UserId.Value, ct);
    }
}