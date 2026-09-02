using QrAssignment.Application.Abstractions; 
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Commands.Excel.BulkCreate
{
    public class BulkCreateAppRoleCommand : ICommand<Result<List<Guid>>>
    {
        public List<BulkCreateAppRoleInputDto> Items { get; set; } = new();
    }
}