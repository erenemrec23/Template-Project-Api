using MediatR;
using Microsoft.AspNetCore.Identity;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity.App;   // AppUser
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Commands.Delete
{
    public sealed class DeleteAppUserCommandHandler : IRequestHandler<DeleteAppUserCommand, Result>
    {
        private readonly IAppUserRepository _userRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IAppLocalizer _localizer;

        public DeleteAppUserCommandHandler(UserManager<AppUser> userManager, 
            IAppLocalizer localizer,
            IAppUserRepository userRepository)
        {
            _userManager = userManager;
            _localizer = localizer;
            _userRepository = userRepository;
        }

        public async Task<Result> Handle(DeleteAppUserCommand request, CancellationToken cancellationToken)
        {
             await _userRepository.DeleteById(request.Id.Value, cancellationToken);
            //if (user is null)
            //    return Result.Failure(new Error("Error.UserNotFound", _localizer["Error.UserNotFound"]));

            //var result = await _userManager.DeleteAsync(user);
            //if (!result.Succeeded)
            //    return Result.Failure(new Error(
            //        "Error.UserCanNotDeleted",
            //        string.Format(
            //            _localizer["Error.UserCanNotDeleted"],
            //            string.Join(", ", result.Errors.Select(e => e.Description)))));

            return Result.Success();
        }
    }
}
