using QrAssignment.Application.Abstractions;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Profile.Commands.Update
{
    public sealed record UpdateProfileCommand(string FirstName, string LastName, string Email) : ICommand<Result>;
}