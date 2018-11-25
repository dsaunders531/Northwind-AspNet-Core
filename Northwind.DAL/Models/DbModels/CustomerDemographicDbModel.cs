using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using mezzanine.Attributes;

namespace Northwind.DAL.Models
{
    [Table("CustomerDemographics")]
    public partial class CustomerDemographicDbModel
    {
        public CustomerDemographicDbModel()
        {
            CustomerCustomerDemo = new HashSet<CustomerCustomerDemoDbModel>();
        }

        [Key]
        [Column("CustomerTypeID")]
        [MaxLength(10)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [SqlInjectionCheck]
        public string CustomerTypeId { get; set; }

        [Column("CustomerDesc", TypeName = "ntext")]
        [SqlInjectionCheck]
        public string CustomerDesc { get; set; }

        public ICollection<CustomerCustomerDemoDbModel> CustomerCustomerDemo { get; set; }
    }
}
