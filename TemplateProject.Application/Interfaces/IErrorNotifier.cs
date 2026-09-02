namespace QrAssignment.Application.Interfaces;

public sealed record ErrorNotification(
    DateTimeOffset OccurredAt,
    string TraceId,
    string Method,
    string Path,
    string? QueryString,
    string? UserName,
    string? ClientIp,
    string ExceptionType,
    string Message,
    string Details);

public interface IErrorNotifier
{
    /// <summary>Non-blocking. Kuyruk doluysa veya özellik kapalıysa false döner; request'i asla bekletmez.</summary>
    bool TryEnqueue(ErrorNotification notification);
}