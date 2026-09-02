using MediatR;
using Microsoft.AspNetCore.Identity;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Commands.Excel.BulkCreate
{
    public class BulkCreateAppUserCommandHandler
        : IRequestHandler<BulkCreateAppUserCommand, Result<List<Guid>>>
    {
        private readonly UserManager<AppUser> _userManager;

        public BulkCreateAppUserCommandHandler(UserManager<AppUser> userManager)
            => _userManager = userManager;

        public async Task<Result<List<Guid>>> Handle(BulkCreateAppUserCommand request, CancellationToken cancellationToken)
        {
            if (request.Items is null || request.Items.Count == 0)
                return Result.Failure<List<Guid>>(
                    new Error("APPUSER_BULK_CREATE_NO_DATA", "Yüklenecek geçerli bir veri bulunamadı."));

            var createdIds = new List<Guid>();
            var failed = new List<string>();

            foreach (var item in request.Items)
            {
                var user = AppUser.Create(item.FirstName, item.LastName, item.UserName, item.Email);

                // Varsayılan geçici şifre belirleyebilir veya CreateAsync(user) kullanabilirsin.
                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                    createdIds.Add(user.Id);
                else
                    failed.Add($"{item.UserName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            if (failed.Count > 0)
                return Result.Failure<List<Guid>>(
                    new Error("APPUSER_BULK_CREATE_PARTIAL", string.Join(" | ", failed)));

            return Result.Success(createdIds);
        }
    }
}