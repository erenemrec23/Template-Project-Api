using MediatR;
using Microsoft.AspNetCore.Identity;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Commands.SetPassive
{
    internal sealed class SetPassiveAppRoleCommandHandler : IRequestHandler<SetPassiveAppRoleCommand, Result>
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IAppLocalizer _localizer;
        private readonly IAppRoleRepository _appRoleRepository;

        public SetPassiveAppRoleCommandHandler(RoleManager<AppRole> roleManager, 
            IAppLocalizer localizer,
            IAppRoleRepository appRoleRepository)
        {
            _roleManager = roleManager;
            _localizer = localizer;
        }

        public async Task<Result> Handle(SetPassiveAppRoleCommand request, CancellationToken cancellationToken)
        {
            //var role = await _roleManager.FindByIdAsync(request.Id.ToString()!);
            //if (role is null)
            //    return Result.Failure(new Error("Error.RoleNotFound", _localizer["Error.RoleNotFound"]));

            //role.IsPassived = false;

            //var updateResult = await _roleManager.UpdateAsync(role);
            //if (!updateResult.Succeeded)
            //    return Result.Failure(new Error(
            //        "Error.RoleCanNotUpdated",
            //        string.Format(
            //            _localizer["Error.RoleCanNotUpdated"],
            //            string.Join(", ", updateResult.Errors.Select(e => e.Description)))));

            await _appRoleRepository.SetPassiveById(request.Id.Value, cancellationToken);
            return Result.Success();
        }
    }
}