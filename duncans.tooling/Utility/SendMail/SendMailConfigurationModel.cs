// <copyright file="SendMailConfigurationModel.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using duncans.Filters;
using System.ComponentModel.DataAnnotations;

namespace duncans.Utility
{
    public class SendMailConfigurationModel
    {
        [SqlInjectionCheck]
        [Required]
        [Display(Description = "The address of the mail server", Prompt = "", ShortName = "The address of the mail server", Name = "The address of the mail server")]
        public string MailHost { get; set; }

        [Required]
        [Range(minimum: 1, maximum: 30000)]
        public int Port { get; set; }

        [Required]
        [RegularExpression(pattern: @"(?!(^[.-].*|[^@]*[.-]@|.*\.{2,}.*)|^.{254}.)([a-zA-Z0-9!#$%&'*+\/=?^_`{|}~.-]+@)(?!-.*|.*-\.)([a-zA-Z0-9-]{1,63}\.)+[a-zA-Z]{2,15}", ErrorMessage = "A valid email address is required.")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [RegularExpression(pattern: @"(?!(^[.-].*|[^@]*[.-]@|.*\.{2,}.*)|^.{254}.)([a-zA-Z0-9!#$%&'*+\/=?^_`{|}~.-]+@)(?!-.*|.*-\.)([a-zA-Z0-9-]{1,63}\.)+[a-zA-Z]{2,15}", ErrorMessage = "A valid email address is required.")]
        public string DefaultSenderAddress { get; set; }

        /// <summary>
        /// Gets or sets the email address for messages from the identity subsystem for monitoring.
        /// </summary>
        [Display(Description = "The mail address where the system messages are sent.", Prompt = "", ShortName = "Send system mail messages here", Name = "Send system mail messages here")]
        [Required]
        [RegularExpression(pattern: @"(?!(^[.-].*|[^@]*[.-]@|.*\.{2,}.*)|^.{254}.)([a-zA-Z0-9!#$%&'*+\/=?^_`{|}~.-]+@)(?!-.*|.*-\.)([a-zA-Z0-9-]{1,63}\.)+[a-zA-Z]{2,15}", ErrorMessage = "A valid email address is required.")]
        [DataType(DataType.EmailAddress)]
        public string MailSystemMessagesTo { get; set; }
    }
}
