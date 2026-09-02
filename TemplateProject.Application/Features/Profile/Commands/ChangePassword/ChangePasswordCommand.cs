using QrAssignment.Application.Abstractions;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Profile.Commands
{
    public sealed record ChangePasswordCommand(string CurrentPassword, string NewPassword) : ICommand<Result>;
}