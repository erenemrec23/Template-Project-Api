using MiniExcelLibs;
using QrAssignment.Application.Common.Excel;
using QrAssignment.Application.Interfaces;   // IAppLocalizer

namespace QrAssignment.Infrastructure.Excel;

public sealed class ExcelDataExportGenerator : IExcelDataExportGenerator
{
    private readonly IAppLocalizer _localizer;
    public ExcelDataExportGenerator(IAppLocalizer localizer) => _localizer = localizer;

    public byte[] Generate<TDto>(IEnumerable<TDto> data) where TDto : class
    {
        var columns = ExcelColumnResolver.Resolve<TDto>(_localizer);

        var rows = new List<Dictionary<string, object>>();
        foreach (var item in data)
        {
            var row = new Dictionary<string, object>(columns.Count);
            foreach (var col in columns)
                row[col.Header] = col.Property.GetValue(item)?.ToString() ?? "";

            rows.Add(row);
        }

        // Veri boş olsa bile başlık satırı görünsün (MiniExcel kolonları ilk satırdan çıkarır)
        if (rows.Count == 0)
            rows.Add(columns.ToDictionary(c => c.Header, _ => (object)""));

        using var stream = new MemoryStream();
        MiniExcel.SaveAs(stream, rows);
        return stream.ToArray();
    }
}