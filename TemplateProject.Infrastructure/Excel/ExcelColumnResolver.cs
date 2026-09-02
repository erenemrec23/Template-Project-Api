using System.Reflection;
using QrAssignment.Application.Attributes;
using QrAssignment.Application.Common.Excel;
using QrAssignment.Application.Interfaces;   // IAppLocalizer

namespace QrAssignment.Infrastructure.Excel;

internal static class ExcelColumnResolver
{
    internal sealed record ExcelColumn(PropertyInfo Property, string Header, bool IncludeInSample);

    public static List<ExcelColumn> Resolve<TDto>(IAppLocalizer localizer)
        => typeof(TDto).GetProperties()
            .Select(p => new { Property = p, Attr = p.GetCustomAttribute<ExcelColumnAttribute>() })
            .Where(x => x.Attr is not null)
            .OrderBy(x => x.Attr!.Order)
            .Select(x => new ExcelColumn(
                x.Property,
                localizer[x.Attr!.LocalizationKey],
                x.Attr!.IncludeInSample))
            .ToList();
}