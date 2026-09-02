using QrAssignment.Application.Abstractions;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Commands.BulkSetPassive
{
    public class BulkSetPassiveTenantCommand : IdListValidationBase,ICommand<Result> 
    {

        public new required List<Guid> IdList { get; set; }
    }
}

