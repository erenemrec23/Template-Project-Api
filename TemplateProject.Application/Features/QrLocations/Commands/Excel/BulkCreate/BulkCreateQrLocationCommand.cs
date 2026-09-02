using QrAssignment.Application.Abstractions;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.QrLocations.Commands.Excel.BulkCreate
{
    public class BulkCreateQrLocationCommand : ICommand<Result<List<Guid>>>
    {
        public List<BulkCreateQrLocationInputDto> Items { get; set; } = new();
    }
}
