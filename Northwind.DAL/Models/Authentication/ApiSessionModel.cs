using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.DAL.Models.Authentication
{
    /// <summary>
    /// Session model to use with the api
    /// </summary>
    [NotMapped]
    public class ApiSessionModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Token { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime SessionStarted { get; set; }
    }
}
