using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QrAssignment.Application.Features.AuthFeatures.Commands.Login;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity.App;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace QrAssignment.Infrastructure.Authentication
{
    internal sealed class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _jwtOptions;
        private readonly IAppUserRefreshTokenRepository _appUserRefreshTokenRepository;
        private readonly IAppUserClaimRepository _appUserClaimRepository;

        public JwtProvider(IOptions<JwtOptions> jwtOptions,
            UserManager<AppUser> userManager,
            IAppUserRefreshTokenRepository appUserRefreshTokenRepository,
            IAppUserClaimRepository appUserClaimRepository)
        {
            _jwtOptions = jwtOptions.Value;
            _appUserRefreshTokenRepository = appUserRefreshTokenRepository;
            _appUserClaimRepository = appUserClaimRepository;
        }

        public async Task<LoginCommandResponse> CreateTokenAsync(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim("FullName", user.FullName),
                new Claim("TenantId", user.TenantId.ToString())
            };

            if (user.AppUserRoles != null && user.AppUserRoles.Any())
            {
                foreach (var role in user.AppUserRoles)
                    claims.Add(new Claim(ClaimTypes.Role, role.AppRole.Name));
            }

            // Kendi + rol yetkileri tek çağrıda, sayfa bazında OR'lanmış olarak gelir
            var effectivePermissions = await _appUserClaimRepository
                .GetEffectivePagePermissionsAsync(user.Id);

            if (effectivePermissions.Any())
            {
                var allPermissionsJson = JsonSerializer.Serialize(
                    effectivePermissions.Select(p => new
                    {
                        pageName = p.PageName,
                        permissionValue = p.PermissionValue
                    }));

                claims.Add(new Claim("permissions", allPermissionsJson));
            }

            DateTime expires = DateTime.Now.AddHours(1);

            JwtSecurityToken jwtSecurityToken = new(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: expires,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
                    SecurityAlgorithms.HmacSha256));

            string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            DateTime refreshTokenExpires = expires.AddMinutes(15);
            string refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

            if (user.RefreshToken is null)
            {
                user.RefreshToken = new AppUserRefreshToken
                {
                    AppUserId = user.Id,
                    RefreshToken = refreshToken,
                    RefreshTokenExpires = refreshTokenExpires
                };
                await _appUserRefreshTokenRepository.AddAsync(user.RefreshToken);
            }
            else
            {
                user.RefreshToken.RefreshToken = refreshToken;
                user.RefreshToken.RefreshTokenExpires = refreshTokenExpires;
                _appUserRefreshTokenRepository.Update(user.RefreshToken);
            }

            return new LoginCommandResponse(
                token,
                refreshToken,
                refreshTokenExpires,
                user.Id.ToString());
        }
    }
}