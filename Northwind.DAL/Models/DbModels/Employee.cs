using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.DAL.Models
{
    [Table("Employees")]
    public partial class Employee
    {
        public Employee()
        {
            EmployeeTerritories = new HashSet<EmployeeTerritory>();
            InverseReportsToNavigation = new HashSet<Employee>();
            Orders = new HashSet<Order>();
        }

        [Key]
        [Column("EmployeeID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }

        [Required]
        [MaxLength(20)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(10)]
        public string FirstName { get; set; }

        [MaxLength(30)]
        public string Title { get; set; }

        [MaxLength(25)]
        public string TitleOfCourtesy { get; set; }

        [DataType(DataType.Date)]
        [Column("BirthDate", TypeName = "datetime")]
        public DateTime? BirthDate { get; set; }

        [DataType(DataType.Date)]
        [Column("HireDate", TypeName = "datetime")]
        public DateTime? HireDate { get; set; }

        [MaxLength(60)]
        public string Address { get; set; }

        [MaxLength(15)]
        public string City { get; set; }

        [MaxLength(15)]
        public string Region { get; set; }

        [DataType(DataType.PostalCode)]
        [MaxLength(10)]
        public string PostalCode { get; set; }

        [MaxLength(15)]
        public string Country { get; set; }

        [MaxLength(24)]
        [DataType(DataType.PhoneNumber)]
        public string HomePhone { get; set; }

        [MaxLength(4)]
        public string Extension { get; set; }

        [Column("Photo", TypeName = "image")]
        public byte[] Photo { get; set; }

        [Column("Notes", TypeName = "ntext")]
        public string Notes { get; set; }

        public int? ReportsTo { get; set; }

        [MaxLength(255)]
        public string PhotoPath { get; set; }

        public Employee ReportsToNavigation { get; set; }
        public ICollection<EmployeeTerritory> EmployeeTerritories { get; set; }
        public ICollection<Employee> InverseReportsToNavigation { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
