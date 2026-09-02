using MediatR;
using QrAssignment.Application.Services;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.AuthFeatures.Commands.DisableUserTwoFactor
{
    internal sealed class AdminDisableUserTwoFactorCommandHandler
        : IRequestHandler<DisableUserTwoFactorCommand, Result>
    {
        private readonly ITwoFactorService _twoFactorService;

        public AdminDisableUserTwoFactorCommandHandler(ITwoFactorService twoFactorService)
        {
            _twoFactorService = twoFactorService;
        }

        public Task<Result> Handle(DisableUserTwoFactorCommand request, CancellationToken ct)
            => _twoFactorService.DisableAsync(request.UserId, ct);
    }
}