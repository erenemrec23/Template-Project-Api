using QrAssignment.Application.Abstractions;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Commands.Excel.BulkCreate
{
    public class BulkCreateAppUserCommand : ICommand<Result<List<Guid>>>
    {
        public List<BulkCreateAppUserInputDto> Items { get; set; } = new();
    }
}