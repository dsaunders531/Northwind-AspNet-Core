using mezzanine.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.DAL.Models
{
    /// <summary>
    /// App configuration seed data user model used by Identity.
    /// </summary>
    [NotMapped]
    public class IdentityUserConfigurationModel
    {
        [SqlInjectionCheck]
        public string Name { get; set; }

        [SqlInjectionCheck]
        public string Email { get; set; }

        [SqlInjectionCheck]
        public string Password { get; set; }
        public string[] Roles { get; set; }
        public IdentityRelationship Relationship { get; set; }
    }
}
