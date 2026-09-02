using MediatR;
using Microsoft.AspNetCore.Identity;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Commands.Excel.BulkCreate
{
    public class BulkCreateAppRoleCommandHandler
        : IRequestHandler<BulkCreateAppRoleCommand, Result<List<Guid>>>
    {
        private readonly RoleManager<AppRole> _roleManager;
        public BulkCreateAppRoleCommandHandler(RoleManager<AppRole> roleManager)
            => _roleManager = roleManager;

        public async Task<Result<List<Guid>>> Handle(BulkCreateAppRoleCommand request, CancellationToken cancellationToken)
        {
            if (request.Items is null || request.Items.Count == 0)
                return Result.Failure<List<Guid>>(
                    new Error("APPROLE_BULK_CREATE_NO_DATA", "Yüklenecek geçerli bir veri bulunamadı."));

            var createdIds = new List<Guid>();
            var failed = new List<string>();

            foreach (var item in request.Items)
            {
                var role = new AppRole { Name = item.Name };
                await _roleManager.CreateAsync(role);   // yalnızca rol kaydı — claim yok

                createdIds.Add(role.Id);
            }

            if (failed.Count > 0)
                return Result.Failure<List<Guid>>(
                    new Error("APPROLE_BULK_CREATE_PARTIAL", string.Join(" | ", failed)));

            return Result.Success(createdIds);
        }
    }
}