using QrAssignment.Application.Common.Excel;
using QrAssignment.Application.Features.QrLocations.Commands.Excel.BulkCreate;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;

public class BulkCreateQrLocationNameUniquenessValidator : IExcelRowBusinessValidator<BulkCreateQrLocationInputDto>
{
    private readonly IQrLocationRepository _qrLocationRepository;

    private readonly IAppLocalizer _localizer;
    public BulkCreateQrLocationNameUniquenessValidator(IQrLocationRepository qrLocationRepository, IAppLocalizer localizer)
    {
        _qrLocationRepository = qrLocationRepository;
        _localizer = localizer;
    }

    public async Task ValidateAsync(
        IReadOnlyList<ExcelRowResultDto<BulkCreateQrLocationInputDto>> rows,
        CancellationToken cancellationToken)
    {
        var candidates = rows
            .Where(r => r.IsValid && r.Data != null && !string.IsNullOrWhiteSpace(r.Data.Name))
            .ToList();

        if (candidates.Count == 0)
            return;

        var names = candidates
            .Where(w => !w.Data!.Code!.HasValue)
            .Select(r => r.Data!.Name!)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var existing = await _qrLocationRepository.GetByNamesAsync(names, cancellationToken);

        var existingNames = new HashSet<string>(
            existing.Select(e => e.Name),
            StringComparer.OrdinalIgnoreCase);

        foreach (var row in candidates)
        {
            if (existingNames.Contains(row.Data!.Name!))
            {
                row.IsValid = false;
                row.Errors.Add(string.Format(_localizer["Error.QrLocationHasInserted"], row.Data.Name));
            }
        }
    }
}
