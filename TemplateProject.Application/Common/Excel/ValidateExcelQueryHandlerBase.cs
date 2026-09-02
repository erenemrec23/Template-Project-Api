using MediatR;
using MiniExcelLibs;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;
using System.Reflection;

namespace QrAssignment.Application.Common.Excel
{
    public abstract class ValidateExcelQueryHandlerBase<TDto>
    : IRequestHandler<ValidateExcelQuery<TDto>, Result<ExcelValidationResponseDto<TDto>>>
    where TDto : class, new()
    {
        private readonly IEnumerable<IExcelRowBusinessValidator<TDto>> _businessValidators;
        private readonly ILocalizationService _localizationService;

        public ValidateExcelQueryHandlerBase(
            IEnumerable<IExcelRowBusinessValidator<TDto>> businessValidators,
            ILocalizationService localizationService)
        {
            _businessValidators = businessValidators;
            _localizationService = localizationService;
        }

        public async Task<Result<ExcelValidationResponseDto<TDto>>> Handle(ValidateExcelQuery<TDto> request, CancellationToken cancellationToken)
        {
            var lang = request.LanguageCode;

            if (request.FileBytes == null || request.FileBytes.Length == 0)
                return Result.Failure<ExcelValidationResponseDto<TDto>>(
                    new Error(_localizationService.GetValue(lang, "Excel.Error.EmptyFile"), ""));

            var response = new ExcelValidationResponseDto<TDto>();

            // Property -> localize edilmiş başlık eşlemesi (dosya için bir kez hesaplanır)
            var columnMap = typeof(TDto).GetProperties()
                .Select(p => new { Property = p, Column = p.GetCustomAttribute<ExcelColumnAttribute>() })
                .Where(x => x.Column != null)
                .Select(x => new ExcelColumnInfo
                {
                    Property = x.Property,
                    ResolvedTitle = _localizationService.GetValue(lang, x.Column!.LocalizationKey)
                })
                .ToList();

            try
            {
                using var stream = new MemoryStream(request.FileBytes);
                var rawRows = stream.Query(useHeaderRow: true).ToList();
                int rowNumber = 1;

                foreach (IDictionary<string, object> rawRow in rawRows)
                {
                    rowNumber++;

                    // Header eşleşmesini case/boşluk toleranslı yapıyoruz
                    var row = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
                    foreach (var kv in rawRow)
                        row[kv.Key?.Trim() ?? string.Empty] = kv.Value;

                    if (row.Values.All(v => v == null || string.IsNullOrWhiteSpace(v.ToString())))
                        continue;

                    var dto = new TDto();
                    var rowResult = new ExcelRowResultDto<TDto> { RowNumber = rowNumber, Data = dto };

                    foreach (var col in columnMap)
                    {
                        row.TryGetValue(col.ResolvedTitle, out var rawValue);
                        var textValue = rawValue?.ToString()?.Trim();

                        var required = col.Property.GetCustomAttribute<ExcelRequiredAttribute>();
                        if (required != null && string.IsNullOrWhiteSpace(textValue))
                        {
                            rowResult.IsValid = false;
                            rowResult.Errors.Add(Resolve(required, lang, $"'{col.ResolvedTitle}' alanı boş olamaz."));
                            continue;
                        }

                        if (string.IsNullOrWhiteSpace(textValue))
                            continue;

                        var maxLength = col.Property.GetCustomAttribute<ExcelMaxLengthAttribute>();
                        if (maxLength != null && textValue.Length > maxLength.Length)
                        {
                            rowResult.IsValid = false;
                            rowResult.Errors.Add(Resolve(maxLength, lang, $"'{col.ResolvedTitle}' en fazla {maxLength.Length} karakter olabilir."));
                        }

                        var regex = col.Property.GetCustomAttribute<ExcelRegexAttribute>();
                        if (regex != null && !System.Text.RegularExpressions.Regex.IsMatch(textValue, regex.Pattern))
                        {
                            rowResult.IsValid = false;
                            rowResult.Errors.Add(Resolve(regex, lang, $"'{col.ResolvedTitle}' formatı geçersiz."));
                        }

                        if (!TryConvertAndSet(col.Property, dto, textValue, out var conversionError))
                        {
                            rowResult.IsValid = false;
                            rowResult.Errors.Add(conversionError!);
                            continue;
                        }

                        var range = col.Property.GetCustomAttribute<ExcelRangeAttribute>();
                        if (range != null)
                        {
                            var numericValue = Convert.ToDouble(col.Property.GetValue(dto));
                            if (numericValue < range.Min || numericValue > range.Max)
                            {
                                rowResult.IsValid = false;
                                rowResult.Errors.Add(Resolve(range, lang, $"'{col.ResolvedTitle}' {range.Min}-{range.Max} aralığında olmalı."));
                            }
                        }
                    }
                     

                    response.Rows.Add(rowResult);
                }

                CheckDuplicatesInFile(response, columnMap, lang);
                foreach (var validator in _businessValidators)
                    await validator.ValidateAsync(response.Rows, cancellationToken);

                response.TotalRowCount = response.Rows.Count;

                return Result.Success(response);
            }
            catch (Exception ex)
            {
                return Result.Failure<ExcelValidationResponseDto<TDto>>(new Error(
                    $"{_localizationService.GetValue(lang, "Excel.Error.ProcessingFailed")}: {ex.Message}", ""));
            }
        }

        private string Resolve(ExcelValidationAttributeBase attr, string lang, string fallback)
        {
            if (!string.IsNullOrEmpty(attr.ErrorMessageKey))
                return _localizationService.GetValue(lang, attr.ErrorMessageKey);
            return attr.ErrorMessage ?? fallback;
        }

        private void CheckDuplicatesInFile(ExcelValidationResponseDto<TDto> response, List<ExcelColumnInfo> columnMap, string lang)
        {
            foreach (var col in columnMap)
            {
                var uniqueAttr = col.Property.GetCustomAttribute<ExcelUniqueInFileAttribute>();
                if (uniqueAttr == null) continue;

                var groups = response.Rows
                    .Where(r => r.Data != null)
                    .GroupBy(r => col.Property.GetValue(r.Data)?.ToString())
                    .Where(g => !string.IsNullOrEmpty(g.Key) && g.Count() > 1);

                foreach (var group in groups)
                    foreach (var row in group)
                    {
                        row.IsValid = false;
                        row.Errors.Add(Resolve(uniqueAttr, lang, $"'{col.ResolvedTitle}' değeri dosya içinde birden fazla kez kullanılmış."));
                    }
            }
        }

        private static bool TryConvertAndSet(PropertyInfo property, TDto dto, string textValue, out string? error)
        {
            // önceki mesajdaki implementasyonla aynı, değişiklik yok
            error = null;
            var targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            try
            {
                object converted = targetType switch
                {
                    Type t when t == typeof(string) => textValue,
                    Type t when t == typeof(long) => long.Parse(textValue),
                    Type t when t == typeof(int) => int.Parse(textValue),
                    Type t when t == typeof(decimal) => decimal.Parse(textValue),
                    Type t when t == typeof(double) => double.Parse(textValue),
                    Type t when t == typeof(DateTime) => DateTime.Parse(textValue),
                    Type t when t == typeof(bool) => bool.Parse(textValue),
                    _ => Convert.ChangeType(textValue, targetType)
                };
                property.SetValue(dto, converted);
                return true;
            }
            catch
            {
                error = $"'{property.Name}' alanı '{textValue}' değeriyle dönüştürülemedi.";
                return false;
            }
        }

        private class ExcelColumnInfo
        {
            public PropertyInfo Property { get; set; } = null!;
            public string ResolvedTitle { get; set; } = string.Empty;
        }
    }
}
