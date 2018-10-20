using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Northwind.DAL.Models
{
    [Table("CustomerDemographics")]
    public partial class CustomerDemographic
    {
        public CustomerDemographic()
        {
            CustomerCustomerDemo = new HashSet<CustomerCustomerDemo>();
        }

        [Key]
        [Column("CustomerTypeID")]
        [MaxLength(10)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string CustomerTypeId { get; set; }

        [Column("CustomerDesc", TypeName = "ntext")]
        public string CustomerDesc { get; set; }

        public ICollection<CustomerCustomerDemo> CustomerCustomerDemo { get; set; }
    }
}
