using QrAssignment.Application.Abstractions;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Commands.Delete
{
    public sealed record DeleteAppUserCommand(Guid? Id) : ICommand<Result>, IdValidationBase;
}
