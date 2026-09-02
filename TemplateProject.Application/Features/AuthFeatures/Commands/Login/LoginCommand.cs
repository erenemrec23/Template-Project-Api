using QrAssignment.Application.Abstractions;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.AuthFeatures.Commands.Login
{
    public sealed record LoginCommand(
    string UserNameOrEmail,
    string Password) : ICommand<Result<LoginCommandResponse>>;
}
