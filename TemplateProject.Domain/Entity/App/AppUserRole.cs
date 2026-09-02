namespace QrAssignment.Domain.Entity.App
{
    // Kullanıcı-Rol join tablosu. Artık HİÇBİR Identity tipinden türemiyor — düz join entity.
    // (Önceden yanlışlıkla IdentityRole<Guid>'den türüyordu; bu, tabloya anlamsız
    //  Id / Name / NormalizedName / ConcurrencyStamp kolonları getiriyordu.)
    public class AppUserRole
    {
        public Guid? AppUserId { get; set; }
        public AppUser? AppUser { get; set; } = null!;

        public Guid? AppRoleId { get; set; }
        public AppRole? AppRole { get; set; } = null!;

  
    }
}