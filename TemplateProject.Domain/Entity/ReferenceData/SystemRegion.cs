using QrAssignment.Domain.Abstractions;

namespace QrAssignment.Domain.Entity.System
{
    public class SystemRegion : BaseEntity
    {
        public required string Name { get; set; }
        public string? Code { get; set; } 

        public SystemRegionLevel Level { get; set; }  

        public Guid? ParentRegionId { get; set; }
        public virtual SystemRegion? ParentRegion { get; set; }
        public virtual ICollection<SystemRegion> SubLocations { get; set; } = new List<SystemRegion>();
    }

}
