using MediatR;
using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Features.Tenants.Commands.Excel.BulkCreate;
using QrAssignment.Application.Features.Tenants.Commands.Excel.Dtos;
using QrAssignment.Domain.Shared;


namespace QrAssignment.Application.Features.Tenants.Commands.Excel.Validate
{
    public class ValidateTenantExcelQuery : ICommand<Result<ExcelValidationResponseDto<BulkCreateTenantInputDto>>>
    { 
        public byte[] FileBytes { get; set; } = Array.Empty<byte>();
    }
}
