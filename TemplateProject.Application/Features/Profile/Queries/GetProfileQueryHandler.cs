using MediatR;
using Microsoft.AspNetCore.Identity;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Services;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Profile.Queries
{
    internal sealed class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, Result<ProfileDto>>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITwoFactorService _twoFactor;

        public GetProfileQueryHandler(ICurrentUserService currentUser, UserManager<AppUser> userManager, ITwoFactorService twoFactor)
        {
            _currentUser = currentUser;
            _userManager = userManager;
            _twoFactor = twoFactor;
        }

        public async Task<Result<ProfileDto>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(_currentUser.UserId.ToString());
            if (user is null)
                return Result.Failure<ProfileDto>(new Error("User.NotFound", "Kullanici bulunamadi."));

            var enabled = await _twoFactor.IsEnabledAsync(_currentUser.UserId.Value, cancellationToken);
            return Result.Success(new ProfileDto(user.FirstName, user.LastName, user.Email!, enabled));
        }
    }
}