using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Northwind.DAL.Attributes;
using mezzanine.Attributes;
using Northwind.DAL.Models;
using mezzanine.EF;

namespace Northwind.BLL.Models
{
    [NotMapped]
    public class EmployeeRowApiModel : ApiModel<int>, IEmployee
    {
        [Required]
        [MaxLength(20)]
        [SqlInjectionCheck]
        public string LastName { get; set; }

        [Required]
        [MaxLength(10)]
        [SqlInjectionCheck]
        public string FirstName { get; set; }

        [MaxLength(30)]
        [SqlInjectionCheck]
        public string Title { get; set; }

        [MaxLength(25)]
        [SqlInjectionCheck]
        public string TitleOfCourtesy { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? HireDate { get; set; }

        [MaxLength(60)]
        [SqlInjectionCheck]
        public string Address { get; set; }

        [MaxLength(15)]
        [SqlInjectionCheck]
        public string City { get; set; }

        [MaxLength(15)]
        [SqlInjectionCheck]
        public string Region { get; set; }

        [Required]
        [DataType(DataType.PostalCode)]
        [MaxLength(10)]
        [SqlInjectionCheck]
        public string PostalCode { get; set; }

        [MaxLength(15)]
        public string Country { get; set; }

        [MaxLength(24)]
        [DataType(DataType.PhoneNumber)]
        [SqlInjectionCheck]
        public string HomePhone { get; set; }

        [MaxLength(4)]
        [SqlInjectionCheck]
        public string Extension { get; set; }

        public byte[] Photo { get; set; }

        [SqlInjectionCheck]
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        [ValidEmployee()]
        public int? ReportsTo { get; set; }

        [SqlInjectionCheck]
        [MaxLength(255)]
        public string PhotoPath { get; set; }
    }
}
