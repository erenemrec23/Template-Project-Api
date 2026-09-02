using QrAssignment.Application.Abstractions;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Commands.Excel.BulkCreate
{ 
    public class BulkCreateTenantCommand : ICommand<Result<List<Guid>>>
    {

        public List<BulkCreateTenantInputDto> Items { get; set; } = new();
    }
}