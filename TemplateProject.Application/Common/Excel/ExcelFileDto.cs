namespace QrAssignment.Application.Common.Excel;

public sealed class ExcelFileDto
{
    public byte[] Data { get; set; } = default!;
    public string FileName { get; set; } = default!;
    public string ContentType { get; set; } = default!;
}