using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Northwind.DAL.Models
{
    /// <summary>
    /// The customer Role model.
    /// </summary>
    public class IdentityRoleModel : IdentityRole<Guid>
    {
        [Required]
        [EnumDataType(typeof(IdentityRoleZone))]
        public IdentityRoleZone Zone { get; set; }
    }
}
