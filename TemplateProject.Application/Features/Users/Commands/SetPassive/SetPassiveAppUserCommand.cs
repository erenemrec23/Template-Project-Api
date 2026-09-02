using QrAssignment.Application.Abstractions;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Commands.SetPassive
{
    public sealed record SetPassiveAppUserCommand(Guid? Id) : ICommand<Result>, IdValidationBase;
}