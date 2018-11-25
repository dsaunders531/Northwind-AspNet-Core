using mezzanine.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.DAL.Models
{
    [Table("CustomerCustomerDemo")]
    public partial class CustomerCustomerDemoDbModel
    {
        [Key]
        [Column("CustomerID")]
        [MaxLength(5)]
        [SqlInjectionCheck]
        public string CustomerId { get; set; }

        [Column("CustomerTypeID")]
        [MaxLength(10)]
        [SqlInjectionCheck]
        public string CustomerTypeId { get; set; }

        public CustomerDbModel Customer { get; set; }
        public CustomerDemographicDbModel CustomerType { get; set; }
    }
}
