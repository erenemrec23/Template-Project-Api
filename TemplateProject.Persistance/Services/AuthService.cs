using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using QrAssignment.Application.Common;
using QrAssignment.Application.Features.AuthFeatures.Commands.Login;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
using QrAssignment.Application.Services;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Exceptions;
using QrAssignment.Domain.Shared;
using System.Text;

namespace QrAssignment.Persistance.Services
{
    internal sealed class AuthService : IAuthService
    {
        private readonly IAppUserRepository _userRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtProvider _jwtProvider;
        private readonly IAppLocalizer _localizer;
        private readonly IEmailService _emailService;
        private readonly IOptions<MailSettings> _mailSettings;
        private readonly ITenantIdService _tenantService;
        private readonly ITwoFactorService _twoFactorService;   
        public AuthService(IAppUserRepository userRepository,
            UserManager<AppUser> userManager,
            IJwtProvider jwtProvider,
            IAppLocalizer localizer, 
            ITenantIdService tenantService,
            IEmailService emailService,
            IOptions<MailSettings> mailSettings ,
            ITwoFactorService twoFactorService)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _jwtProvider = jwtProvider; 
            _localizer = localizer;
            _emailService = emailService;
            _mailSettings = mailSettings;
            _tenantService = tenantService;
            _twoFactorService = twoFactorService;
        }

        public async Task<LoginCommandResponse> LoginAsync(string email, string password, CancellationToken cancellationToken)
        { 
            AppUser? user = await _userRepository.GetByEmailWithRefreshTokenAsync(email, cancellationToken);
          
            if (user is null)
            {
                throw new BusinessException(_localizer["Messages.UserMailUserPasswordNotFound"]); 
            } 
            bool checkPassword = await _userManager.CheckPasswordAsync(user, password);

            if (!checkPassword)
            {
                throw new BusinessException(_localizer["Messages.UserMailUserPasswordNotFound"]);
            }
            var twoFactorEnabled = await _twoFactorService.IsEnabledAsync(user.Id, cancellationToken);
            if (twoFactorEnabled)
            {
                // Token URETME. Frontend'e "ikinci adim gerekli" de.
                return new LoginCommandResponse(
                    Token: null,
                    RefreshToken: null,
                    RefreshTokenExpires: null,
                    UserId: user.Id.ToString(),
                    RequiresTwoFactor: true);
            }
            var token = await _jwtProvider.CreateTokenAsync(user);

            return token;
        }


        public async Task<Guid> CreateAsync(string firstName, string lastName, string email, string password, CancellationToken cancellationToken)
        { 
            var existingUser = await _userRepository.GetByEmailWithRefreshTokenAsync(email);
            if (existingUser is not null)
            {
                throw new BusinessException(_localizer["Messages.MailAlreadyExists"]);
            }
             
            AppUser user = new()
            { 
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                UserName = email, 
                PasswordHash = _userManager.PasswordHasher.HashPassword(null, password),
            };
             
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            { 
                var error = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new BusinessException(error);
            }
            return user.Id;
        }
        public async Task<Guid> UpdateAsync(Guid id,string firstName, string lastName, string email, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByEmailWithRefreshTokenAsync(id, email);
            if (existingUser is not null)
                throw new BusinessException(_localizer["Messages.MailAlreadyExists"]);

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null)
                throw new BusinessException(_localizer["Error.UserNotFound"]);
            user.FirstName = firstName;
            user.LastName = lastName;
            user.Email = email;
            user.UserName = email;  
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var error = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new BusinessException(error);
            }
            return user.Id;
        }

        public async Task<Result> ForgotPasswordAsync(string email, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailForRememberPasswordAsync(email);

            // Kullanici enumeration'ini engellemek icin: kullanici bulunamasa bile basarili donuyoruz.
            if (user is null)
                return Result.Success();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            // Token URL query'sinde tasinacagi icin encode ediyoruz (+ / = karakterleri bozulmasin).
            var resetLink =
                $"{_mailSettings.Value.ClientUrl}/reset-password" +
                $"?token={encodedToken}" +
                $"&email={Uri.EscapeDataString(email)}" +
                $"&token={Uri.EscapeDataString(token)}";

            const string subject = "Şifre Sıfırlama Talebi";
            var body = $@"
        <p>Merhaba,</p>
        <p>Hesabınız için şifre sıfırlama talebinde bulunuldu. Aşağıdaki bağlantıya tıklayarak yeni şifrenizi belirleyebilirsiniz:</p>
        <p><a href=""{resetLink}"">Şifremi Sıfırla</a></p>
        <p>Bu talebi siz yapmadıysanız bu e-postayı yok sayabilirsiniz.</p>";

            await _emailService.SendEmailAsync(email, subject, body, cancellationToken);

            return Result.Success();
        }
        public async Task<Result> ResetPasswordAsync(string email, string token, string newPassword, CancellationToken cancellationToken)
        { 
             
            var info = await _userRepository.GetByEmailForRememberPasswordAsync(email, cancellationToken); // sadece Guid? döner
            if (info is null)
                return Result.Failure(new Error("RESET_PASSWORD_INVALID", "Şifre sıfırlama işlemi geçersiz."));

            if (info.TenantId is Guid tid)
                _tenantService.SetTenantId(tid);
             
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Result.Failure(new Error("RESET_PASSWORD_INVALID", "Şifre sıfırlama işlemi geçersiz."));

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, newPassword);

            return Result.Success();
        }
        public async Task<Result> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                return Result.Failure(new Error("User.NotFound", "Kullanici bulunamadi."));

            // Mevcut sifreyi dogrular; yanlissa PasswordMismatch doner.
            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!result.Succeeded)
                return Result.Failure(new Error("User.ChangePassword", string.Join(" ", result.Errors.Select(e => e.Description))));

            return Result.Success();
        }
        
        public async Task<LoginCommandResponse> IssueTokenForUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            AppUser? user = await _userRepository.GetByIdWithRefreshTokenAsync(userId, cancellationToken);

            if (user is null)
                throw new BusinessException(_localizer["Messages.UserMailUserPasswordNotFound"]);

            // LoginAsync ile birebir ayni token uretimi.
            var token = await _jwtProvider.CreateTokenAsync(user);
            return token;
        }
    }
}
