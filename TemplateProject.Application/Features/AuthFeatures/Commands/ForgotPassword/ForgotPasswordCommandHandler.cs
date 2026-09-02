using MediatR;
using QrAssignment.Application.Services;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.AuthFeatures.Commands.ForgotPassword
{
    public sealed class ForgotPasswordCommandHandler
        : IRequestHandler<ForgotPasswordCommand, Result>
    {
        private readonly IAuthService _authService;

        public ForgotPasswordCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            // IAuthService zaten Result donduruyor; LoginHandler'daki gibi sarmalamaya gerek yok.
            return await _authService.ForgotPasswordAsync(request.Email, cancellationToken);
        }
    }
}