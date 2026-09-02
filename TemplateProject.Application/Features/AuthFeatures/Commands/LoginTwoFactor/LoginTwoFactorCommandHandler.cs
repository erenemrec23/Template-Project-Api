 
using MediatR;
using QrAssignment.Application.Features.AuthFeatures.Commands.Login;
using QrAssignment.Application.Services;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.AuthFeatures.Commands.LoginTwoFactor
{
    public sealed class LoginTwoFactorCommandHandler
        : IRequestHandler<LoginTwoFactorCommand, Result<LoginCommandResponse>>
    {
        private readonly ITwoFactorService _twoFactorService;
        private readonly IAuthService _authService;

        public LoginTwoFactorCommandHandler(ITwoFactorService twoFactorService, IAuthService authService)
        {
            _twoFactorService = twoFactorService;
            _authService = authService;
        }

        public async Task<Result<LoginCommandResponse>> Handle(LoginTwoFactorCommand request, CancellationToken ct)
        {
            var valid = await _twoFactorService.VerifyCodeAsync(request.UserId, request.Code, ct);
            if (!valid)
                return Result.Failure<LoginCommandResponse>(new Error("TwoFactor.InvalidCode", "Dogrulama kodu hatali."));

            // Kod dogru -> artik token uret. Bunun icin IAuthService'e userId ile token ureten bir metot lazim.
            var response = await _authService.IssueTokenForUserAsync(request.UserId, ct);
            return Result.Success(response);
        }
    }
}