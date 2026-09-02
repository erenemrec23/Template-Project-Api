using QrAssignment.Domain.Abstractions;
using QrAssignment.Domain.Attributes;

namespace QrAssignment.Domain.Entity
{
    public class QrLocation : TenantBaseEntity
    {
        [Filterable]
        public required string Name { get; set; }

        [Filterable]
        public DateTimeOffset? StartDate { get; set; }

        [Filterable]
        public DateTimeOffset? EndDate { get; set; }

        [Filterable]
        public string? LocationName { get; set; }
         
    }

}
