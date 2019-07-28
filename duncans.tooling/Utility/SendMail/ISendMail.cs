// <copyright file="ISendMail.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace duncans.Utility
{
    public interface ISendMail : IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage, SendMailAttachment attachment, bool deleteAttachmentsAfterSend);
    }
}