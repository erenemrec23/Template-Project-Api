using QrAssignment.Application.Abstractions;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Commands.BulkDelete
{
    public class BulkDeleteTenantCommand : IdListValidationBase,ICommand<Result> 
    {

        public new required List<Guid> IdList { get; set; }
    }
}

