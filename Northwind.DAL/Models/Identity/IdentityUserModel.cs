using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Northwind.DAL.Models
{
    /// <summary>
    /// The custom IdentityUser model.
    /// </summary>
    public class IdentityUserModel : IdentityUser<Guid>
    {
        [Required]
        [EnumDataType(typeof(IdentityRelationship))]
        public IdentityRelationship Relationship { get; set; }

        public bool PasswordReset { get; set; }
    }
}
