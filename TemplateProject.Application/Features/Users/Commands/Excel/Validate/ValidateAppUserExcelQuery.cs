using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Features.Tenants.Commands.Excel.Dtos;
using QrAssignment.Application.Features.Users.Commands.Excel.BulkCreate;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Commands.Excel.Validate
{
    public class ValidateAppUserExcelQuery
        : ICommand<Result<ExcelValidationResponseDto<BulkCreateAppUserInputDto>>>
    {
        public byte[] FileBytes { get; set; } = Array.Empty<byte>();
    }
}