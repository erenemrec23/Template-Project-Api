namespace QrAssignment.Domain.Abstractions
{
    public interface ISoftDelete
    {
        bool IsPassived { get; set; } 
    }
}
