using QrAssignment.Application.Abstractions;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Commands.BulkSetActive
{
    public class BulkSetActiveAppRoleCommand : IdListValidationBase, ICommand<Result>
    {
        public new required List<Guid> IdList { get; set; }
    }
}


