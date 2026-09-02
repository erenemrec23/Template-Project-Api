using QrAssignment.Application.DTOs.List;
using QrAssignment.Domain.Attributes;
using System.Globalization;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace QrAssignment.Application.Extensions
{
    public static class DynamicQueryableExtensions
    {
        public static IQueryable<T> ToDynamic<T>(this IQueryable<T> query, DynamicQueryDto dynamicQuery)
        {
            if (dynamicQuery.Filter != null)
            {
                var values = new List<object>();
                string whereQuery = Transform(dynamicQuery.Filter, values, typeof(T));

                if (!string.IsNullOrEmpty(whereQuery))
                {
                    query = query.Where(whereQuery, values.ToArray());
                }
            }

            if (dynamicQuery.Sort != null && dynamicQuery.Sort.Any())
            {
                string ordering = string.Join(",", dynamicQuery.Sort.Select(s => $"{s.Field} {s.Dir}"));
                query = query.OrderBy(ordering);
            }

            return query;
        }
        private static PropertyInfo? ResolveProperty(Type entityType, string path)
        {
            PropertyInfo? property = null;
            var currentType = entityType;

            foreach (var part in path.Split('.'))
            {
                property = currentType.GetProperty(part);
                if (property is null)
                    return null;
                 
                currentType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            }

            return property;
        }
        private static string Transform(DynamicQueryFilterDto filter, List<object> values, Type entityType)
        {

            string comparison = string.Empty;

            if (!string.IsNullOrEmpty(filter.Field) && !string.IsNullOrEmpty(filter.Operator))
            {
                var property = ResolveProperty(entityType, filter.Field);   // eskiden entityType.GetProperty(filter.Field)
                if (property == null)
                    throw new ArgumentException($"'{filter.Field}' alanı bulunamadı.");

                bool isFilterable = property.GetCustomAttributes(typeof(FilterableAttribute), inherit: true).Any();
                if (!isFilterable)
                    throw new UnauthorizedAccessException($"'{filter.Field}' alanı üzerinden filtreleme yapılamaz.");

                if (string.Equals(filter.Operator, "between", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(filter.Value) || string.IsNullOrEmpty(filter.Value2))
                        throw new ArgumentException($"'{filter.Field}' alanı için 'between' operatöründe hem Value hem Value2 zorunludur.");

                    object startValue = ConvertValue(filter.Value, property.PropertyType);
                    object endValue = GetEndOfDay(ConvertValue(filter.Value2, property.PropertyType));

                    int startIndex = values.Count;
                    values.Add(startValue);

                    int endIndex = values.Count;
                    values.Add(endValue);

                    comparison = $"({filter.Field} >= @{startIndex} && {filter.Field} <= @{endIndex})";
                }
                else
                {
                    string opLower = filter.Operator.ToLower();

                    if (opLower is "isempty" or "isnotempty")
                    {
                        bool isStringField = property.PropertyType == typeof(string);
                         
                        bool isNonNullableValueType = property.PropertyType.IsValueType
                            && Nullable.GetUnderlyingType(property.PropertyType) == null;

                        if (!isStringField && isNonNullableValueType)
                        {
                            comparison = opLower == "isempty" ? "(1 == 2)" : "(1 == 1)";
                        }
                        else
                        {
                            comparison = opLower == "isempty"
                                ? (isStringField
                                    ? $"({filter.Field} == null || {filter.Field} == \"\")"
                                    : $"{filter.Field} == null")
                                : (isStringField
                                    ? $"({filter.Field} != null && {filter.Field} != \"\")"
                                    : $"{filter.Field} != null");
                        }
                    }
                    else
                    {
                        var underlyingType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                        bool isDateType = underlyingType == typeof(DateTime) || underlyingType == typeof(DateTimeOffset);

                        // Tarih alanlarında (DateTime/DateTimeOffset) veritabanındaki değer saat bilgisi
                        // taşıyor ama frontend'den sadece tarih ("2026-07-17") geliyor, yani her zaman
                        // günün başına (00:00) denk geliyor. Bu yüzden:
                        //  - "eq"  hiçbir zaman eşleşmiyordu    -> gün aralığına (>= başı && <= sonu) çevrildi
                        //  - "neq" her zaman true dönüyordu     -> gün aralığının dışı olarak çevrildi
                        //  - "gt"  o günü de yanlışlıkla dahil ediyordu -> günün SONUYLA karşılaştırılıyor
                        //  - "lte" o günü neredeyse hariç tutuyordu    -> günün SONUYLA karşılaştırılıyor
                        // "gte" ve "lt" zaten gün başı (00:00) ile doğru çalıştığı için dokunulmadı.
                        if (isDateType && opLower is "eq" or "neq" or "gt" or "lte")
                        {
                            object dayStart = ConvertValue(filter.Value, property.PropertyType);
                            object dayEnd = GetEndOfDay(dayStart);

                            switch (opLower)
                            {
                                case "eq":
                                    {
                                        int startIndex = values.Count;
                                        values.Add(dayStart);
                                        int endIndex = values.Count;
                                        values.Add(dayEnd);
                                        comparison = $"({filter.Field} >= @{startIndex} && {filter.Field} <= @{endIndex})";
                                        break;
                                    }
                                case "neq":
                                    {
                                        int startIndex = values.Count;
                                        values.Add(dayStart);
                                        int endIndex = values.Count;
                                        values.Add(dayEnd);
                                        comparison = $"({filter.Field} < @{startIndex} || {filter.Field} > @{endIndex})";
                                        break;
                                    }
                                case "gt":
                                    {
                                        int endIndex = values.Count;
                                        values.Add(dayEnd);
                                        comparison = $"{filter.Field} > @{endIndex}";
                                        break;
                                    }
                                case "lte":
                                    {
                                        int endIndex = values.Count;
                                        values.Add(dayEnd);
                                        comparison = $"{filter.Field} <= @{endIndex}";
                                        break;
                                    }
                            }
                        }
                        else
                        {
                            int index = values.Count;
                            comparison = GetComparison(filter.Operator, filter.Field, index, property.PropertyType);
                            values.Add(ConvertValue(filter.Value, property.PropertyType));
                        }
                    }
                }
            }

            if (filter.Filters != null && filter.Filters.Any())
            {
                string logic = filter.Logic ?? "and";
                var subFilters = new List<string>();

                foreach (var subFilter in filter.Filters)
                {
                    var subTransformed = Transform(subFilter, values, entityType);
                    if (!string.IsNullOrEmpty(subTransformed))
                    {
                        subFilters.Add(subTransformed);
                    }
                }

                if (subFilters.Any())
                {
                    string subFilterString = string.Join($" {logic} ", subFilters);

                    if (!string.IsNullOrEmpty(comparison))
                    {
                        return $"({comparison} {logic} ({subFilterString}))";
                    }

                    return $"({subFilterString})";
                }
            }

            return comparison;
        }

        /// <summary>
        /// Bir tarih değerinin (DateTime/DateTimeOffset) günün son anını (23:59:59.9999999) döner.
        /// Frontend'den gelen tarihler her zaman günün başına (00:00) denk geldiği için,
        /// "o güne kadar/o gün dahil" gibi karşılaştırmalarda kullanılır.
        /// Tarih tipi değilse (örn. int, string) değeri değiştirmeden geri döner.
        /// </summary>
        private static object GetEndOfDay(object dateValue)
        {
            return dateValue switch
            {
                DateTime dt => dt.Date.AddDays(1).AddTicks(-1),
                DateTimeOffset dto => new DateTimeOffset(dto.Date.AddDays(1).AddTicks(-1), dto.Offset),
                _ => dateValue
            };
        }

        private static string GetComparison(string op, string field, int index, Type propertyType)
        {
            bool isString = propertyType == typeof(string);
            string target = isString ? field : $"{field}.ToString()";

            return op.ToLower() switch
            {
                "eq" => $"{field} == @{index}",
                "neq" => $"{field} != @{index}",
                "gt" => $"{field} > @{index}",
                "gte" => $"{field} >= @{index}",
                "lt" => $"{field} < @{index}",
                "lte" => $"{field} <= @{index}",
                "startswith" => $"{target}.StartsWith(@{index})",
                "endswith" => $"{target}.EndsWith(@{index})",
                "contains" => $"{target}.Contains(@{index})",
                "doesnotcontain" => $"!{target}.Contains(@{index})",
                _ => $"{field} == @{index}"
            };
        }

        /// <summary>
        /// Gelen string değeri (filter.Value), hedef property'nin gerçek tipine çevirir.
        /// "eq"/"gt" gibi operatörlerde Dynamic LINQ'in tip uyuşmazlığından patlamaması için gereklidir.
        /// "contains" gibi string bazlı operatörlerde zaten target alan ToString()'e çevrildiği için
        /// value'yu string olarak bırakmak yeterlidir.
        /// </summary>
        private static object ConvertValue(string? value, Type propertyType)
        {
            if (value is null) return null!;

            var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

            try
            {
                if (underlyingType == typeof(string)) return value;
                if (underlyingType == typeof(Guid)) return Guid.Parse(value);
                if (underlyingType == typeof(bool)) return bool.Parse(value);
                if (underlyingType == typeof(DateTime)) return DateTime.Parse(value, CultureInfo.InvariantCulture);
                if (underlyingType == typeof(DateTimeOffset)) return DateTimeOffset.Parse(value, CultureInfo.InvariantCulture);
                if (underlyingType.IsEnum) return Enum.Parse(underlyingType, value, ignoreCase: true);

                // int, long, decimal, double, float vb. IConvertible tipler
                return Convert.ChangeType(value, underlyingType, CultureInfo.InvariantCulture);
            }
            catch (Exception ex) when (ex is FormatException or OverflowException or InvalidCastException)
            {
                throw new ArgumentException(
                    $"'{value}' değeri '{propertyType.Name}' tipine dönüştürülemedi.", ex);
            }
        }
    }
}