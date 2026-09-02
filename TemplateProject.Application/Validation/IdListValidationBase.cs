namespace QrAssignment.Application.Abstractions;

public abstract class IdListValidationBase
{
    public List<Guid> IdList { get; set; } = new(); 
}