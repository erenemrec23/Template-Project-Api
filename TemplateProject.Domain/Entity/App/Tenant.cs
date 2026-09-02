using QrAssignment.Domain.Abstractions;
using QrAssignment.Domain.Attributes;
using System.ComponentModel.DataAnnotations.Schema;


namespace QrAssignment.Domain.Entity.App
{
    public class Tenant : BaseEntity
    {
        [Filterable]
        public string Name { get; set; }


    }
}

