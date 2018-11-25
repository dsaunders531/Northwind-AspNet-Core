using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using mezzanine.Attributes;
using mezzanine.ViewModel;

namespace Northwind.BLL.Models.Authentication
{
    //[NotMapped]
    //public class LoginViewModel : ViewModel<LoginAppModel>
    //{
    //}

    /// <summary>
    /// View model for logging in.
    /// </summary>
    [NotMapped]
    public class LoginAppModel
    {
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

        public string ReturnUrl { get; set; }
    }
}
