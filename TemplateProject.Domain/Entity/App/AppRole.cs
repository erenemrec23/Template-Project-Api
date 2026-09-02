using Microsoft.AspNetCore.Identity;
using QrAssignment.Domain.Abstractions;
using QrAssignment.Domain.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace QrAssignment.Domain.Entity.App
{

    public class AppRole : IdentityRole<Guid>, IMustHaveTenant, IBaseEntity, ISoftDelete
    {
        public Guid? TenantId { get; set; }
        [Filterable]
        public override string Name { get; set; } = default!;

        public virtual DateTimeOffset CreatedDate { get; set; }
        public virtual DateTimeOffset? ModifiedDate { get; set; }

        public virtual bool IsPassived { get; set; } = false;
        [Filterable]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RevNum { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = null!; 

        public static AppRole Create(string name)
        {
            return new AppRole()
            {
                Name = name
            };
        }


        public Guid? CreatedByUserId { get; set; }
        public AppUser? CreatedByUser { get; set; }

        public Guid? ModifiedByUserId { get; set; }
        public AppUser? ModifiedByUser { get; set; }


        //public virtual ICollection<IdentityRoleClaim<Guid>> Claims { get; set; }
    }
}

