using QrAssignment.Application.Abstractions;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.QrLocations.Commands.BulkSetPassive
{
    public class BulkSetPassiveQrLocationCommand : IdListValidationBase, ICommand<Result>
    {
        public new required List<Guid> IdList { get; set; }
    }
}
