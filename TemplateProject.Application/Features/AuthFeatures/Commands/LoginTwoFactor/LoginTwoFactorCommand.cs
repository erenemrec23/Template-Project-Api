using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Features.AuthFeatures.Commands.Login;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.AuthFeatures.Commands.LoginTwoFactor
{
    // Ilk adimda RequiresTwoFactor=true donen kullanici, kodla bu adima gelir.
    public sealed record LoginTwoFactorCommand(
        Guid UserId,
        string Code) : ICommand<Result<LoginCommandResponse>>; 
}