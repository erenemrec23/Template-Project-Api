using QrAssignment.Application.Common.Excel;
using QrAssignment.Application.Features.Users.Commands.Excel.BulkCreate;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;

namespace QrAssignment.Application.Features.Users.Commands.Excel.BulkCreate
{
    public class BulkCreateAppUserUniquenessValidator : IExcelRowBusinessValidator<BulkCreateAppUserInputDto>
    {
        private readonly IAppUserRepository _appUserRepository;
        private readonly IAppLocalizer _localizer;

        public BulkCreateAppUserUniquenessValidator(IAppUserRepository appUserRepository, IAppLocalizer localizer)
        {
            _appUserRepository = appUserRepository;
            _localizer = localizer;
        }

        public async Task ValidateAsync(
            IReadOnlyList<ExcelRowResultDto<BulkCreateAppUserInputDto>> rows,
            CancellationToken cancellationToken)
        {
            var candidates = rows
                .Where(r => r.IsValid && r.Data != null)
                .ToList();

            if (candidates.Count == 0)
                return;

            var userNames = candidates
                .Where(r => !string.IsNullOrWhiteSpace(r.Data!.UserName))
                .Select(r => r.Data!.UserName!)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var emails = candidates
                .Where(r => !string.IsNullOrWhiteSpace(r.Data!.Email))
                .Select(r => r.Data!.Email!)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var existingUserNames = new HashSet<string>(
                await _appUserRepository.GetExistingUserNamesAsync(userNames, cancellationToken),
                StringComparer.OrdinalIgnoreCase);

            var existingEmails = new HashSet<string>(
                await _appUserRepository.GetExistingEmailsAsync(emails, cancellationToken),
                StringComparer.OrdinalIgnoreCase);

            foreach (var row in candidates)
            {
                if (existingUserNames.Contains(row.Data!.UserName!))
                {
                    row.IsValid = false;
                    row.Errors.Add(string.Format(_localizer["Error.UserNameHasInserted"], row.Data.UserName));
                }

                if (existingEmails.Contains(row.Data!.Email!))
                {
                    row.IsValid = false;
                    row.Errors.Add(string.Format(_localizer["Error.EmailHasInserted"], row.Data.Email));
                }
            }
        }
    }
}