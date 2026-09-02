namespace QrAssignment.Domain.Exceptions
{
    public sealed class DuplicateEntityException : BusinessException
    {
        public string? EntityName { get; }
        public string? DuplicateValue { get; }

        public DuplicateEntityException(string message, string? entityName = null, string? duplicateValue = null)
            : base(message)
        {
            EntityName = entityName;
            DuplicateValue = duplicateValue;
        }
    }
}
