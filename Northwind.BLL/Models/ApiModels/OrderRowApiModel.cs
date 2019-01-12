using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Northwind.DAL.Attributes;
using mezzanine.Attributes;
using mezzanine.EF;

namespace Northwind.BLL.Models
{
    [NotMapped]
    public class OrderRowApiModel : ApiModel<int>
    {
        [Required]
        [ValidCustomer()]
        [MaxLength(5)]
        [SqlInjectionCheck]
        public int CustomerId { get; set; }

        [Required]
        [ValidEmployee()]
        public int? EmployeeId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? OrderDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? RequiredDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ShippedDate { get; set; }

        public int? ShipVia { get; set; }

        [DataType(DataType.Currency)]
        public decimal? Freight { get; set; }

        [Required]
        [MaxLength(40)]
        [SqlInjectionCheck]
        public string ShipName { get; set; }

        [Required]
        [MaxLength(60)]
        [SqlInjectionCheck]
        public string ShipAddress { get; set; }

        [MaxLength(15)]
        [SqlInjectionCheck]
        public string ShipCity { get; set; }

        [MaxLength(15)]
        [SqlInjectionCheck]
        public string ShipRegion { get; set; }

        [Required]
        [DataType(DataType.PostalCode)]
        [MaxLength(10)]
        [SqlInjectionCheck]
        public string ShipPostalCode { get; set; }

        [MaxLength(15)]
        [SqlInjectionCheck]
        public string ShipCountry { get; set; }
    }
}
