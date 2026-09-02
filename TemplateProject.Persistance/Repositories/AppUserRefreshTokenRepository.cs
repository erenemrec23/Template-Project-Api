using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Persistance.Context;
using QrAssignment.Persistance.Repositories.Base;

namespace QrAssignment.Persistance.Repositories;

internal sealed class AppUserRefreshTokenRepository : GenericRepository<AppUserRefreshToken>, IAppUserRefreshTokenRepository
{
    public AppUserRefreshTokenRepository(AppDbContext context) : base(context)
    {
    }
}
