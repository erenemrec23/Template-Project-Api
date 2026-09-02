using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Features.Tenants.Commands.Excel.Dtos;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Common.Excel
{
    public class ValidateExcelQuery<TDto> : ICommand<Result<ExcelValidationResponseDto<TDto>>>
     where TDto : class, new()
    {
        public byte[] FileBytes { get; set; } = Array.Empty<byte>();
        public string LanguageCode { get; set; } = "tr-TR";
    }
}
