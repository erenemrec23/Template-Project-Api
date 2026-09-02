using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
        Task<Result> ForgotPasswordAsync(string email, CancellationToken cancellationToken);
    }
}
