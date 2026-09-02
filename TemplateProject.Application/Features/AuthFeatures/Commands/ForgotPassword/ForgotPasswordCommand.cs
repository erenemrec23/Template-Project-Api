using QrAssignment.Application.Abstractions;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.AuthFeatures.Commands.ForgotPassword
{
    public sealed record ForgotPasswordCommand(
        string Email) : ICommand<Result>;
}