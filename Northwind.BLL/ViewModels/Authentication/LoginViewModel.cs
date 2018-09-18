using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using mezzanine.ViewModels;

namespace Northwind.BLL.ViewModels.Authentication
{
    /// <summary>
    /// View model for logging in.
    /// </summary>
    [NotMapped]
    public class LoginViewModel : ViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [NotMapped]
        public string ReturnUrl { get; set; }
    }
}
