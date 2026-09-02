using MediatR;
using Microsoft.AspNetCore.Identity;
using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Commands.SetActive
{
    internal sealed class SetActiveAppRoleCommandHandler : IRequestHandler<SetActiveAppRoleCommand, Result>
    { 
        private readonly IAppRoleRepository _appRoleRepository;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppLocalizer _localizer;

        public SetActiveAppRoleCommandHandler( 
            IAppRoleRepository appRoleRepository,
            RoleManager<AppRole> roleManager,
            IUnitOfWork unitOfWork,
            IAppLocalizer localizer)
        { 
            _appRoleRepository = appRoleRepository;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result> Handle(SetActiveAppRoleCommand request, CancellationToken cancellationToken)
        {
            //var role = await _appRoleRepository.GetPassivedByIdAsync(request.Id.Value, cancellationToken);

            //if (role is null)
            //    return Result.Failure(new Error("ROLE_NOT_FOUND", "Rol bulunamadı."));

            //if (!role.IsPassived)
            //    return Result.Failure(new Error("ROLE_ALREADY_ACTIVE", "Rol zaten aktif."));

            //role.IsPassived = false;
            //var updateResult = await _roleManager.UpdateAsync(role);
            //if (!updateResult.Succeeded)
            //    return Result.Failure(new Error(
            //         string.Format(_localizer["Error.RoleUCanNotUpdated"],
            //         string.Join(", ", updateResult.Errors.Select(e => e.Description))), "Error.RoleUCanNotUpdated"));

            await _appRoleRepository.SetActiveAsync(request.Id.Value, cancellationToken);
            return Result.Success("Rol aktifleştirildi.");
        }
    }
}