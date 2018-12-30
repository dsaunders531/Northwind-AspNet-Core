﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using mezzanine.Attributes;
using mezzanine.EF;

namespace Northwind.DAL.Models
{
    [Table("Shippers")]
    public partial class ShipperDbModel : DbModel<int>
    {
        public ShipperDbModel()
        {
            Orders = new HashSet<OrderDbModel>();
            Deleted = false;
            RowVersion = 1;
        }

        [Required]
        [MaxLength(40)]
        [SqlInjectionCheck]
        public string CompanyName { get; set; }

        [MaxLength(24)]
        [DataType(DataType.PhoneNumber)]
        [SqlInjectionCheck]
        public string Phone { get; set; }

        public ICollection<OrderDbModel> Orders { get; set; }
    }
}
