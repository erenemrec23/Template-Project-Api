// QrAssignment.Persistance/Exceptions/SqlServerExceptionTranslator.cs
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace QrAssignment.Persistance.Exceptions;

internal sealed partial class SqlServerExceptionTranslator : IDbExceptionTranslator
{
    private const int UniqueIndexViolation = 2601;
    private const int UniqueConstraintViolation = 2627;
    private const int ForeignKeyViolation = 547;

    // Index/constraint adı → alan adı için localization key
    private static readonly Dictionary<string, string> FieldNameKeys =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["IX_Tenants_Name"] = "Field.Tenant.Name",
        };

    private readonly IAppLocalizer _localizer;

    public SqlServerExceptionTranslator(IAppLocalizer localizer)
        => _localizer = localizer;

    public bool TryTranslate(Exception exception, out Exception translated)
    {
        translated = exception;

        if (exception is not DbUpdateException { InnerException: SqlException sql })
            return false;

        switch (sql.Number)
        {
            case UniqueIndexViolation:
            case UniqueConstraintViolation:
                translated = BuildDuplicate(sql, exception);
                return true;

            case ForeignKeyViolation:
                translated = new BusinessException(
                    _localizer["Error.ForeignKeyViolation"]);
                return true;

            default:
                return false;
        }
    }

    private DuplicateEntityException BuildDuplicate(SqlException sql, Exception inner)
    {
        var name = NameRegex().Match(sql.Message).Groups["name"].Value;
        var value = ValueRegex().Match(sql.Message).Groups["value"].Value;

        // index adı bir key'e maplenmişse localize et; yoksa ham index adını göster
        var field = FieldNameKeys.TryGetValue(name, out var key)
            ? _localizer[key]
            : name;

        var message = string.Format(
            _localizer["Error.DuplicateEntity"], value, field);

        return new DuplicateEntityException(message, field, value);
    }

    [GeneratedRegex(@"(?:index|constraint) '(?<name>[^']+)'")]
    private static partial Regex NameRegex();

    [GeneratedRegex(@"duplicate key value is \((?<value>.*?)\)")]
    private static partial Regex ValueRegex();
}