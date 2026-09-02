using QrAssignment.Application.Abstractions;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Commands.SetActive
{
    public sealed record SetActiveAppUserCommand(Guid? Id) : ICommand<Result>, IdValidationBase;
}