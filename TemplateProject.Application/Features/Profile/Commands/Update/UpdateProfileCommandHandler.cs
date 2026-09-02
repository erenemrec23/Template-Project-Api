using MediatR;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Services;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Profile.Commands.Update
{
    internal sealed class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Result>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IAuthService _authService;

        public UpdateProfileCommandHandler(ICurrentUserService currentUser, IAuthService authService)
        {
            _currentUser = currentUser;
            _authService = authService;
        }

        public async Task<Result> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            await _authService.UpdateAsync(_currentUser.UserId.Value, request.FirstName, request.LastName, request.Email, cancellationToken);
            return Result.Success();
        }
    }
}