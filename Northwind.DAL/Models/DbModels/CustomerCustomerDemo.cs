using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.DAL.Models
{
    public partial class CustomerCustomerDemo
    {
        [Key]
        [Column("CustomerID")]
        [MaxLength(5)]
        public string CustomerId { get; set; }

        [Column("CustomerTypeID")]
        [MaxLength(10)]
        public string CustomerTypeId { get; set; }

        public Customer Customer { get; set; }
        public CustomerDemographic CustomerType { get; set; }
    }
}
