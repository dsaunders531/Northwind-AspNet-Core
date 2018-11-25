using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.DAL.Models
{
    /// <summary>
    /// App configuration seed data model used by Identity.
    /// </summary>
    [NotMapped]
    public class IdentityConfigurationModel
    {
        public List<IdentityRoleConfigurationModel> Roles { get; set; }
        public IdentityUserConfigurationModel AdminUser { get; set; }
        public string[] NewUserRoles { get; set; }
    }
}
