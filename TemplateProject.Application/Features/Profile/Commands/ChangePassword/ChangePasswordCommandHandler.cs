using MediatR;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Services;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Profile.Commands
{
    internal sealed class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IAuthService _authService;

        public ChangePasswordCommandHandler(ICurrentUserService currentUser, IAuthService authService)
        {
            _currentUser = currentUser;
            _authService = authService;
        }

        public Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
            => _authService.ChangePasswordAsync(_currentUser.UserId.Value, request.CurrentPassword, request.NewPassword, cancellationToken);
    }
}