using MiniExcelLibs;
using QrAssignment.Application.Common.Excel;
using QrAssignment.Application.Interfaces;
using System.Reflection; 

namespace QrAssignment.Infrastructure.Excel;

public sealed class ExcelSampleTemplateGenerator : IExcelSampleTemplateGenerator
{
    private readonly IAppLocalizer _localizer;
    public ExcelSampleTemplateGenerator(IAppLocalizer localizer) => _localizer = localizer;

    public byte[] Generate<TDto>(int sampleRowCount = 3) where TDto : class
    {
        var columns = ExcelColumnResolver.Resolve<TDto>(_localizer);   // reflection bloğu yerine

        var rows = new List<Dictionary<string, object>>(sampleRowCount);
        for (int i = 1; i <= sampleRowCount; i++)
        {
            var row = new Dictionary<string, object>(columns.Count);
            foreach (var col in columns)
                row[col.Header] = col.IncludeInSample ? $"{col.Header} {i}" : "";
            rows.Add(row);
        }

        using var stream = new MemoryStream();
        MiniExcel.SaveAs(stream, rows);
        return stream.ToArray();
    }
}