using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Persistance.Context;

namespace QrAssignment.Persistance.Identity.Stores
{
    public sealed class AppUserStore : UserStore<AppUser, AppRole, AppDbContext, Guid>
    {
        public AppUserStore(AppDbContext context, IdentityErrorDescriber? describer = null)
            : base(context, describer) => AutoSaveChanges = false;
    }
}
