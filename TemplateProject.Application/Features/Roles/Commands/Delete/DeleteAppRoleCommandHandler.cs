using MediatR;
using Microsoft.AspNetCore.Identity;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared; 
namespace QrAssignment.Application.Features.Roles.Commands.Delete
{
    // Handler
    public sealed class DeleteAppRoleCommandHandler : IRequestHandler<DeleteAppRoleCommand, Result>
    {
        private readonly RoleManager<
 QrAssignment.Domain.Entity.App.AppRole> _roleManager;
        private readonly IAppLocalizer _localizer;

        private readonly IAppRoleRepository _appRoleRepository;
        public DeleteAppRoleCommandHandler(
            RoleManager<QrAssignment.Domain.Entity.App.AppRole> roleManager,
             IAppLocalizer localizer,
             IAppRoleRepository appRoleRepository)
        {
            _roleManager = roleManager;
            _localizer = localizer;
            _appRoleRepository = appRoleRepository;
        }

        public async Task<Result> Handle(DeleteAppRoleCommand request, CancellationToken cancellationToken)
        { 
            
             await _appRoleRepository.DeleteById(request.Id.Value, cancellationToken);
            return Result.Success();
        }
    }
}