using Microsoft.AspNetCore.Http;
using QrAssignment.Application.Interfaces;
using System.Security.Claims;
using System.Text.Json;
namespace QrAssignment.Presentation.Services
{
    public sealed class CurrentUserService : ICurrentUserService
    {
        private static readonly JsonSerializerOptions JsonOpts =
            new() { PropertyNameCaseInsensitive = true };

        private readonly IHttpContextAccessor _httpContextAccessor;
         
        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public Guid? UserId => Guid.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : null;

        // GetClaim yerine GetClaims yaptık ve dönüş tipini IEnumerable<string> olarak değiştirdik
        public IEnumerable<string> GetClaims(string claimType)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null)
            {
                // Null reference hatalarını önlemek için boş liste dönüyoruz
                return Enumerable.Empty<string>();
            }

            // FindAll metodu ile hem User'dan hem Role'den gelen tüm aynı isimli claimleri yakalıyoruz
            return user.FindAll(claimType).Select(c => c.Value);
        }
        public string GetClaim(string claimType)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null)
            {
                // Null reference hatalarını önlemek için boş liste dönüyoruz
                return string.Empty;
            }

            // FindAll metodu ile hem User'dan hem Role'den gelen tüm aynı isimli claimleri yakalıyoruz
            return user.FindFirst(claimType)?.Value ?? string.Empty;
        }

        public IReadOnlyDictionary<string, int> GetPagePermissions()
        {
            // Frontend bu claim'i JSON.parse ediyordu -> claim, JSON string'e
            // serialize edilmis bir dizidir: [{ "pageName": "...", "permissionValue": 7 }, ...]
            var raw = User?.FindFirst("permissions")?.Value;
            if (string.IsNullOrWhiteSpace(raw))
                return new Dictionary<string, int>();

            var list = JsonSerializer.Deserialize<List<PagePermissionClaim>>(raw, JsonOpts)
                       ?? new List<PagePermissionClaim>();

            var dict = new Dictionary<string, int>(StringComparer.Ordinal);
            foreach (var p in list)
                dict[p.PageName] = p.PermissionValue;
            return dict;
        }

        private sealed record PagePermissionClaim(string PageName, int PermissionValue);
    }
}