using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Persistance.Context;
// AppDbContext namespace'ini ekle

public sealed class AppRoleStore : RoleStore<AppRole, AppDbContext, Guid>
{
    public AppRoleStore(AppDbContext context, IdentityErrorDescriber? describer = null)
        : base(context, describer) => AutoSaveChanges = false;
}

public sealed class AppUserStore : UserStore<AppUser, AppRole, AppDbContext, Guid>
{
    public AppUserStore(AppDbContext context, IdentityErrorDescriber? describer = null)
        : base(context, describer) => AutoSaveChanges = false;
}