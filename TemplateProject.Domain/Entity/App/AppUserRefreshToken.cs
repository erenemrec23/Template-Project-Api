using QrAssignment.Domain.Abstractions;

namespace QrAssignment.Domain.Entity.App
{
    public sealed class AppUserRefreshToken  : BaseEntity
    { 
        public Guid AppUserId { get; set; }
        public string RefreshToken { get; set; } = null!;
        public DateTime RefreshTokenExpires { get; set; }
         
        public AppUser AppUser { get; set; } = null!;

        public static AppUserRefreshToken Create(Guid appUserId, string refreshToken, DateTime refreshTokenExpires)
        {
            return new AppUserRefreshToken()
            {
                AppUserId = appUserId,
                RefreshToken = refreshToken,
                RefreshTokenExpires = refreshTokenExpires
            };
        }
    }
}
