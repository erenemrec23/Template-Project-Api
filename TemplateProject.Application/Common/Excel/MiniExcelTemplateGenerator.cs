using QrAssignment.Application.Interfaces;
using System.Reflection;

namespace QrAssignment.Application.Common.Excel
{

    public class MiniExcelTemplateGenerator : IExcelTemplateGenerator
    {
        private readonly ILocalizationService _localizationService;
        public MiniExcelTemplateGenerator(ILocalizationService localizationService)
            => _localizationService = localizationService;

        public byte[] Generate<TDto>(string languageCode) where TDto : class, new()
        {
            var headers = typeof(TDto).GetProperties()
                .Select(p => p.GetCustomAttribute<ExcelColumnAttribute>())
                .Where(a => a != null)
                .Select(a => _localizationService.GetValue(languageCode, a!.LocalizationKey))
                .ToList();

            var emptyRow = headers.ToDictionary(h => h, h => (object?)string.Empty);

            using var stream = new MemoryStream();
            MiniExcelLibs.MiniExcel.SaveAs(stream, new[] { emptyRow });
            return stream.ToArray();
        }
    }
}
