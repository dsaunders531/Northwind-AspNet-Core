﻿using duncans.EF;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.DAL.Models
{
    [Table("CustomerCustomerDemo")]
    public partial class CustomerCustomerDemoDbModel : DbModel<long>
    {
        public CustomerCustomerDemoDbModel()
        {
            Deleted = false;
            RowVersion = 1;
        }

        [Column("CustomerID")]        
        public int CustomerId { get; set; }

        [Column("CustomerTypeID")]
        public long CustomerTypeId { get; set; }

        public CustomerDbModel Customer { get; set; }
        public CustomerDemographicDbModel CustomerType { get; set; }
    }
}