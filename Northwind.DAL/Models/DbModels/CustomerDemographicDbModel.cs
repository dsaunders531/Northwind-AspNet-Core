using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using mezzanine.Attributes;
using mezzanine.EF;

namespace Northwind.DAL.Models
{
    [Table("CustomerDemographics")]
    public partial class CustomerDemographicDbModel : DbModel<long>
    {
        public CustomerDemographicDbModel()
        {
            CustomerCustomerDemo = new HashSet<CustomerCustomerDemoDbModel>();
            Deleted = false;
            RowVersion = 1;
        }

        [Column("CustomerDesc", TypeName = "ntext")]
        [SqlInjectionCheck]
        public string CustomerDesc { get; set; }

        public ICollection<CustomerCustomerDemoDbModel> CustomerCustomerDemo { get; set; }
    }
}
