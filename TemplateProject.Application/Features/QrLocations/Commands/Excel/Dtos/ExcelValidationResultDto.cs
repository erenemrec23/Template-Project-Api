namespace QrAssignment.Application.Features.QrLocations.Commands.Excel.Dtos
{
    // NOT: ExcelValidationResponseDto<TDto> ve ExcelRowErrorDto generic/paylaşımlı tiplerdir
    // ve ...Tenants.Commands.Excel.Dtos altında bir kez tanımlıdır; burada yeniden
    // tanımlanmaz (aksi halde base handler'ın döndürdüğü tiple çakışır).
    // Bu dosya yalnızca entity'ye özel satır sonucu DTO'sunu barındırır.
    public class ExcelQrLocationRowResultDto
    {
        public int RowNumber { get; set; }
        public long? Code { get; set; }
        public string? Name { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string? LocationName { get; set; }
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
