using QrAssignment.Domain.Abstractions;
using QrAssignment.Domain.Attributes;
using QrAssignment.Domain.Entity.System;

namespace QrAssignment.Domain.Entity
{
    public class QrApplicant : TenantBaseEntity
    {
        [Filterable]
        public required string FirstName { get; set; }
        [Filterable]
        public required string LastName { get; set; }
        [Filterable]
        public required string Mail { get; set; }

        [Filterable]
        public string? TCKN { get; set; }
         
        public Guid? RegionId { get; set; }
        public virtual SystemRegion? Region { get; set; }
    }
}