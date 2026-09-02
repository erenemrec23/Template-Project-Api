using ClosedXML.Excel;
using Microsoft.Extensions.Localization; // Projendeki çeviri arayüzüne göre değişebilir
using QrAssignment.Application.Attributes;
using System.Reflection;

namespace QrAssignment.Application.Helpers
{
    public static class ExcelExportHelper
    {
        public static byte[] GenerateExcel<T>(
            IEnumerable<T> data,
            string sheetName,
            IStringLocalizer localizer) // Çeviri servisini dışarıdan alıyoruz
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(sheetName);

            // 1. REFLECTION İLE DTO'Yİ OKU VE SIRALA
            var type = typeof(T);
            var properties = type.GetProperties()
                .Select(p => new
                {
                    Property = p,
                    Attribute = p.GetCustomAttribute<ColumnDisplayAttribute>()
                })
                .Where(x => x.Attribute != null) // Sadece etiketi olanları al
                .OrderBy(x => x.Attribute.Order) // Order değerine göre soldan sağa sırala
                .ToList();

            // 2. BAŞLIKLARI YAZ VE ÇEVİRİLERİ GETİR
            for (int i = 0; i < properties.Count; i++)
            {
                var propInfo = properties[i].Property;

                // Tam istediğin formatta Key üretimi:
                // Örnek: ExcelColumnTitle_QrAssignment_Application_Features_Tenants_Queries_Export_TenantListItemExcelDto_Name
                string namespacePart = type.Namespace?.Replace(".", "_") ?? "";
                string localizationKey = $"ExcelColumnTitle_{namespacePart}_{type.Name}_{propInfo.Name}";

                // Key'i çeviri servisinden geçirip hücreye yaz
                worksheet.Cell(1, i + 1).Value = localizer[localizationKey].Value;
            }
            worksheet.Row(1).Style.Font.Bold = true;

            // 3. VERİLERİ DİNAMİK OLARAK YAZ
            int rowIndex = 2;
            foreach (var item in data)
            {
                for (int colIndex = 0; colIndex < properties.Count; colIndex++)
                {
                    var propInfo = properties[colIndex].Property;
                    // Reflection ile nesnenin o anki property değerini okuyoruz
                    var val = propInfo.GetValue(item);
                    worksheet.Cell(rowIndex, colIndex + 1).Value = val?.ToString() ?? string.Empty;
                }
                rowIndex++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return stream.ToArray();
        }
    }
}