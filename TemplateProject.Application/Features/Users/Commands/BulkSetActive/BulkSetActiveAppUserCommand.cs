using QrAssignment.Application.Abstractions;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Commands.BulkSetActive
{
    public class BulkSetActiveAppUserCommand : IdListValidationBase, ICommand<Result>
    {
        public new required List<Guid> IdList { get; set; }
    }
}
