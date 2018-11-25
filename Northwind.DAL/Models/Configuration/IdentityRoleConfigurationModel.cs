using mezzanine.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Northwind.DAL.Models
{
    [NotMapped]
    public class IdentityRoleConfigurationModel
    {
        [SqlInjectionCheck]
        public string Name { get; set; }

        public IdentityRoleZone Zone { get; set; }
    }
}
