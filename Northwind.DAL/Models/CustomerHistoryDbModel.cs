using duncans.EF;
using duncans.Filters;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.DAL.Models
{
    [Table("CustomersHistory")]
    public class CustomerHistoryDbModel : HistoricDbModel<int>, ICustomer
    {
        public CustomerHistoryDbModel()
        {
            Deleted = false;
            RowVersion = 1;
            StartDate = DateTime.Now;
        }

        [Column("CustomerID")]
        [MaxLength(5)]
        [Required]
        [SqlInjectionCheck]
        public string CustomerKey { get; set; }

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
    }
}
