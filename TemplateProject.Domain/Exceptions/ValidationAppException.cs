namespace QrAssignment.Domain.Exceptions;

public sealed class ValidationAppException : Exception
{
    // Hataları PropertyName ve Hata Mesajları şeklinde tutarız
    public Dictionary<string, string[]> Errors { get; }

    public ValidationAppException(Dictionary<string, string[]> errors)
        : base("Validasyon hatası oluştu.")
    {
        Errors = errors;
    }
}