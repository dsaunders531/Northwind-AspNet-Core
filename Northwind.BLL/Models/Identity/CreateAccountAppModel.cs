using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using mezzanine.Attributes;
using mezzanine.ViewModel;

namespace Northwind.BLL.Models.Authentication
{
    /// <summary>
    /// View model for creating an account.
    /// </summary>
    [NotMapped]
    public class RegisterAccountAppModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Your Name")]
        [MinLength(4, ErrorMessage ="The name is too short!")]
        [MaxLength(256, ErrorMessage = "The name is too long!")]
        [SqlInjectionCheck]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        [SqlInjectionCheck]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [SqlInjectionCheck]
        public string Password { get; set; }

        [NotMapped]
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Please enter your password again")]
        [Compare("Password", ErrorMessage = "The passwords do not match")]
        [SqlInjectionCheck]
        public string Password2 { get; set; }

        [NotMapped]
        [SqlInjectionCheck]
        public string ReturnUrl { get; set; }
    }
}
