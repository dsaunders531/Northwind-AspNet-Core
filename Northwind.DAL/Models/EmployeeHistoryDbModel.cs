﻿using duncans.EF;
using duncans.Filters;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.DAL.Models
{
    [Table("EmployeesHistory")]
    public class EmployeeHistoryDbModel : HistoricDbModel<int>, IEmployee
    {
        public EmployeeHistoryDbModel()
        {
            Deleted = false;
            RowVersion = 1;
            StartDate = DateTime.Now;
        }

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

        [DataType(DataType.Date)]
        [Column("BirthDate", TypeName = "datetime")]
        public DateTime? BirthDate { get; set; }

        [DataType(DataType.Date)]
        [Column("HireDate", TypeName = "datetime")]
        public DateTime? HireDate { get; set; }

        [MaxLength(60)]
        [SqlInjectionCheck]
        public string Address { get; set; }

        [MaxLength(15)]
        [SqlInjectionCheck]
        public string City { get; set; }

        [MaxLength(15)]
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
        public string HomePhone { get; set; }

        [MaxLength(4)]
        [SqlInjectionCheck]
        public string Extension { get; set; }

        [Column("Photo", TypeName = "image")]
        public byte[] Photo { get; set; }

        [Column("Notes", TypeName = "ntext")]
        [SqlInjectionCheck]
        public string Notes { get; set; }

        public int? ReportsTo { get; set; }

        [MaxLength(255)]
        [SqlInjectionCheck]
        public string PhotoPath { get; set; }       
    }
}