using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Features.QrLocations.Commands.Excel.BulkCreate;
using QrAssignment.Application.Features.Tenants.Commands.Excel.Dtos;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.QrLocations.Commands.Excel.Validate
{
    // NOT: ExcelValidationResponseDto<T> generic olduğu için Tenant altındaki
    // ...Tenants.Commands.Excel.Dtos namespace'inden yeniden kullanılıyor
    // (base handler'ın döndürdüğü tip ile birebir eşleşmesi için).
    public class ValidateQrLocationExcelQuery : ICommand<Result<ExcelValidationResponseDto<BulkCreateQrLocationInputDto>>>
    {
        public byte[] FileBytes { get; set; } = Array.Empty<byte>();
    }
}
