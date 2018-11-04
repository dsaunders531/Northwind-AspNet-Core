using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [NotMapped]
        public string ReturnUrl { get; set; }
    }
}
