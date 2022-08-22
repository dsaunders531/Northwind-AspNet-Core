using Northwind.BLL.Validators;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.BLL.Models
{
    [NotMapped]
    public class OrderRowApiO
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        [ValidCustomer()]
        [MaxLength(5)]
        public string CustomerId { get; set; }

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
        public string ShipName { get; set; }

        [Required]
        [MaxLength(60)]
        public string ShipAddress { get; set; }

        [MaxLength(15)]
        public string ShipCity { get; set; }

        [MaxLength(15)]
        public string ShipRegion { get; set; }

        [Required]
        [DataType(DataType.PostalCode)]
        [MaxLength(10)]
        public string ShipPostalCode { get; set; }

        [MaxLength(15)]
        public string ShipCountry { get; set; }
    }
}
