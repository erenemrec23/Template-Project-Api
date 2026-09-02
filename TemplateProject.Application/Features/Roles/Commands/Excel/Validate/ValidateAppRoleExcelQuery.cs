using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Features.Roles.Commands.Excel.BulkCreate;
using QrAssignment.Application.Features.Tenants.Commands.Excel.Dtos; // ExcelValidationResponseDto burada
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Commands.Excel.Validate
{
    public class ValidateAppRoleExcelQuery
        : ICommand<Result<ExcelValidationResponseDto<BulkCreateAppRoleInputDto>>>
    {
        public byte[] FileBytes { get; set; } = Array.Empty<byte>();
    }
}