namespace QrAssignment.Application.Interfaces
{
    public interface IUserContext
    {
        Guid? GetCurrentUserId();
    }
}
