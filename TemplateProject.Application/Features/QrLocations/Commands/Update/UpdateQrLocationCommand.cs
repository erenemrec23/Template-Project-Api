using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.QrLocations.Commands.Update
{
    public class UpdateQrLocationCommand : ICommand<Result<UpdateQrLocationResponse>>, IdValidationBase
    {
        public Guid? Id { get; set; }
        public required string Name { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string? LocationName { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
