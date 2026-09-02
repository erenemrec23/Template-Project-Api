using QrAssignment.Application.Common.Excel;
using QrAssignment.Application.Features.Tenants.Commands.Excel.BulkCreate;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
public class BulkCreateTenantNameUniquenessValidator : IExcelRowBusinessValidator<BulkCreateTenantInputDto>
{
    private readonly ITenantRepository _tenantRepository;

    private readonly IAppLocalizer _localizer;
    public BulkCreateTenantNameUniquenessValidator(ITenantRepository tenantRepository, IAppLocalizer localizer)
    {
        _tenantRepository = tenantRepository;
        _localizer = localizer;
    }

    public async Task ValidateAsync(
        IReadOnlyList<ExcelRowResultDto<BulkCreateTenantInputDto>> rows,
        CancellationToken cancellationToken)
    { 
        var candidates = rows
            .Where(r => r.IsValid && r.Data != null && !string.IsNullOrWhiteSpace(r.Data.Name))
            .ToList();

        if (candidates.Count == 0)
            return;

        var names = candidates
            .Where(w=> !w.Data!.Code!.HasValue)
            .Select(r => r.Data!.Name!)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var existing = await _tenantRepository.GetByNamesAsync(names, cancellationToken);

        var existingNames = new HashSet<string>(
            existing.Select(e => e.Name),
            StringComparer.OrdinalIgnoreCase);

        foreach (var row in candidates)
        {
            if (existingNames.Contains(row.Data!.Name!))
            {
                row.IsValid = false;
                row.Errors.Add(string.Format(_localizer["Error.TenantHasInserted"], row.Data.Name));
            }
        }
    }
}