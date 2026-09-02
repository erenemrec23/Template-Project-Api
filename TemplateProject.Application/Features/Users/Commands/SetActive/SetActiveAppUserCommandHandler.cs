using MediatR;
using Microsoft.AspNetCore.Identity;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Commands.SetActive
{
    internal sealed class SetActiveAppUserCommandHandler : IRequestHandler<SetActiveAppUserCommand, Result>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAppLocalizer _localizer;
        private readonly IAppUserRepository _appUserRepository;

        public SetActiveAppUserCommandHandler(UserManager<AppUser> userManager, 
            IAppLocalizer localizer,
            IAppUserRepository appUserRepository)
        {
            _userManager = userManager;
            _localizer = localizer;
            _appUserRepository = appUserRepository;
        }

        public async Task<Result> Handle(SetActiveAppUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _appUserRepository.GetPassivedByIdAsync(request.Id.Value, cancellationToken);
            if (user is null)
                return Result.Failure(new Error("Error.UserNotFound", _localizer["Error.UserNotFound"]));

            user.IsPassived = false;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return Result.Failure(new Error(
                    "Error.UserCanNotUpdated",
                    string.Format(
                        _localizer["Error.UserCanNotUpdated"],
                        string.Join(", ", updateResult.Errors.Select(e => e.Description)))));

            return Result.Success();
        }
    }
}