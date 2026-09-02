using QrAssignment.Application.Features.AuthFeatures.Commands.Login;
using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Application.Interfaces
{
    public interface IJwtProvider
    {
        Task<LoginCommandResponse> CreateTokenAsync(AppUser user); // User Domain'den gelecek
    }
}
