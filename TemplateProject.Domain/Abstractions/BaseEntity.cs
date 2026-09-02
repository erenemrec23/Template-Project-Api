using QrAssignment.Domain.Attributes;
using QrAssignment.Domain.Entity.App;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace QrAssignment.Domain.Abstractions
{

    public interface IBaseEntity: ISoftDelete
    {
        Guid Id { get; set; }

        DateTimeOffset CreatedDate { get; set; }
        DateTimeOffset? ModifiedDate { get; set; }

        Guid? CreatedByUserId { get; set; }
        Guid? ModifiedByUserId { get; set; }

        long RevNum { get; set; } 


        [Timestamp]
        byte[] RowVersion { get; set; }
    }
    public class BaseEntity : IBaseEntity, ISoftDelete
    { 
        public Guid Id { get; set; } = Guid.CreateVersion7();

        [Filterable]
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;

        [Filterable]
        public DateTimeOffset? ModifiedDate { get; set; }
        public  bool IsPassived { get; set; } 

        [Timestamp] 
        public byte[] RowVersion { get; set; } = null!;

        [Filterable]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RevNum { get; set; }

        public Guid? CreatedByUserId { get; set; }
        public AppUser? CreatedByUser { get; set; } 

        public Guid? ModifiedByUserId { get; set; }
        public AppUser? ModifiedByUser { get; set; } 
    }


  
}
