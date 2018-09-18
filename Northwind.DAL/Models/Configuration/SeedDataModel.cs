using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.DAL.Models
{
    /// <summary>
    /// App configuration seed data model used by Identity.
    /// </summary>
    [NotMapped]
    public class SeedDataModel
    {
        public List<string> CreateDefaultRoles { get; set; }
        public SeedDataUserModel AdminUser { get; set; }
        public string[] DefaultRoles { get; set; }
    }
}
