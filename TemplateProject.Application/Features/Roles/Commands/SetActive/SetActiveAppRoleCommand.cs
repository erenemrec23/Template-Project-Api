using QrAssignment.Application.Abstractions;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Commands.SetActive
{
    public sealed record SetActiveAppRoleCommand(Guid? Id) : ICommand<Result>, IdValidationBase;
}