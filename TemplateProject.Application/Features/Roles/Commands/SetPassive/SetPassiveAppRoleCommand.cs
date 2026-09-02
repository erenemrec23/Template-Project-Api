using QrAssignment.Application.Abstractions;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Commands.SetPassive
{
    public sealed record SetPassiveAppRoleCommand(Guid? Id) : ICommand<Result>, IdValidationBase;
}