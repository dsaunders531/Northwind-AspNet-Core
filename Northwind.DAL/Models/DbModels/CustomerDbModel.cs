﻿using mezzanine.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.DAL.Models
{
    [Table("Customers")]
    public partial class CustomerDbModel
    {
        public CustomerDbModel()
        {
            CustomerCustomerDemo = new HashSet<CustomerCustomerDemoDbModel>();
            Orders = new HashSet<OrderDbModel>();
        }

        [Key]
        [Column("CustomerID")]
        [MaxLength(5)]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [SqlInjectionCheck]
        public string CustomerId { get; set; }

        [Required]
        [MaxLength(40)]
        [SqlInjectionCheck]
        public string CompanyName { get; set; }

        [MaxLength(30)]
        [SqlInjectionCheck]
        public string ContactName { get; set; }

        [MaxLength(30)]
        [SqlInjectionCheck]
        public string ContactTitle { get; set; }

        [MaxLength(60)]
        [SqlInjectionCheck]
        public string Address { get; set; }

        [MaxLength(15)]
        [SqlInjectionCheck]
        public string City { get; set; }

        [MaxLength(15)]
        [SqlInjectionCheck]
        public string Region { get; set; }

        [DataType(DataType.PostalCode)]
        [MaxLength(10)]
        [SqlInjectionCheck]
        public string PostalCode { get; set; }

        [MaxLength(15)]
        [SqlInjectionCheck]
        public string Country { get; set; }

        [MaxLength(24)]
        [DataType(DataType.PhoneNumber)]
        [SqlInjectionCheck]
        public string Phone { get; set; }

        [MaxLength(24)]
        [DataType(DataType.PhoneNumber)]
        [SqlInjectionCheck]
        public string Fax { get; set; }

        public ICollection<CustomerCustomerDemoDbModel> CustomerCustomerDemo { get; set; }
        public ICollection<OrderDbModel> Orders { get; set; }
    }
}