﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using mezzanine.Attributes;
using Northwind.BLL.Validators;

namespace Northwind.BLL.Models
{
    [NotMapped]
    public class CustomerRowApiModel
    {
        [Required]
        [MaxLength(5)]
        [SqlInjectionCheck]
        public string CustomerId { get; set; }

        [Required]
        [MaxLength(40)]
        [SqlInjectionCheck]
        public string CompanyName { get; set; }

        [MaxLength(30)]
        [SqlInjectionCheck]
        public string ContactName { get; set; }

        [Required]
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

        [Required]
        [DataType(DataType.PostalCode)]
        [MaxLength(15)]
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