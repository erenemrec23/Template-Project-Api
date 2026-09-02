namespace QrAssignment.Application.Interfaces
{
    public interface IDbExceptionTranslator
    {
        bool TryTranslate(Exception exception, out Exception translated);
    }
}
