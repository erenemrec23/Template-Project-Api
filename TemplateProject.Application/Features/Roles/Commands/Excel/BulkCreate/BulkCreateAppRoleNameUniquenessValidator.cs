using QrAssignment.Application.Common.Excel;
using QrAssignment.Application.Features.Roles.Commands.Excel.BulkCreate;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;

public class BulkCreateAppRoleNameUniquenessValidator : IExcelRowBusinessValidator<BulkCreateAppRoleInputDto>
{
    private readonly IAppRoleRepository _appRoleRepository;
    private readonly IAppLocalizer _localizer;

    public BulkCreateAppRoleNameUniquenessValidator(IAppRoleRepository appRoleRepository, IAppLocalizer localizer)
    {
        _appRoleRepository = appRoleRepository;
        _localizer = localizer;
    }

    public async Task ValidateAsync(
        IReadOnlyList<ExcelRowResultDto<BulkCreateAppRoleInputDto>> rows,
        CancellationToken cancellationToken)
    {
        var candidates = rows
            .Where(r => r.IsValid && r.Data != null && !string.IsNullOrWhiteSpace(r.Data.Name))
            .ToList();

        if (candidates.Count == 0)
            return;

        var names = candidates
            .Select(r => r.Data!.Name!)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var existing = await _appRoleRepository.GetByNamesAsync(names, cancellationToken);

        var existingNames = new HashSet<string>(
            existing.Select(e => e.Name!),
            StringComparer.OrdinalIgnoreCase);

        foreach (var row in candidates)
        {
            if (existingNames.Contains(row.Data!.Name!))
            {
                row.IsValid = false;
                row.Errors.Add(string.Format(_localizer["Error.RoleHasInserted"], row.Data.Name));
            }
        }
    }
}