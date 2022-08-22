using mezzanine.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.BLL.ViewModels.Authentication
{
    /// <summary>
    /// View model for creating an account.
    /// </summary>
    [NotMapped]
    public class CreateAccountViewModel : ViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Your Name")]
        [MinLength(4, ErrorMessage = "The name is too short!")]
        [MaxLength(256, ErrorMessage = "The name is too long!")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [NotMapped]
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Please enter your password again")]
        [Compare("Password", ErrorMessage = "The passwords do not match")]
        public string Password2 { get; set; }

        [NotMapped]
        public string ReturnUrl { get; set; }
    }
}
