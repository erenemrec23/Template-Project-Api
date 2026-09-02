using MediatR;
using QrAssignment.Application.Abstractions;
using QrAssignment.Domain.Shared;
using System.Windows.Input;

namespace QrAssignment.Application.Features.Roles.Commands.Delete
{
    // Command
    public sealed record DeleteAppRoleCommand(Guid? Id) : ICommand<Result>, IdValidationBase;
}