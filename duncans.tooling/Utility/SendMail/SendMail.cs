// <copyright file="SendMail.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

// Using Jeffrey Steadfasts MailKit library https://github.com/jstedfast/MailKit
// See https://github.com/dotnet/platform-compat/blob/master/docs/DE0005.md
// and https://docs.microsoft.com/en-us/dotnet/api/system.net.mail.smtpclient?view=netframework-4.7.2
// See instructions at: http://www.mimekit.net/docs/html/Creating-Messages.htm
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
#if ! DEBUG
    using System;
#endif
using System.IO;
using System.Threading.Tasks;

namespace duncans.Utility
{
    /// <summary>
    /// Creates an instance of the SendMail class.
    /// </summary>
    public sealed class SendMail : IEmailSender, ISendMail
    {
        public SendMail(SendMailConfigurationModel configuration)
        {
            this.Configuration = configuration;
        }

        private SendMailConfigurationModel Configuration { get; set; }

        public Task SendEmailAsync(string from, string to, string subject, string body)
        {
            return Task.Run(() => this.SendEmail(from, to, subject, body, body.Contains("<html>") && body.Contains("</html>"), null));
        }

        public Task SendEmailAsync(string from, string to, string subject, string body, bool isHtmlBody)
        {
            return Task.Run(() => this.SendEmail(from, to, subject, body, isHtmlBody, null));
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.Run(() => this.SendEmail(Configuration.DefaultSenderAddress, email, subject, htmlMessage, true, null));
        }

        public Task SendEmailAsync(string from, string to, string subject, string body, SendMailAttachment attachment)
        {
            return Task.Run(() => this.SendEmail(from, to, subject, body, body.Contains("<html>") && body.Contains("</html>"), attachment));
        }

        public Task SendEmailAsync(string from, string to, string subject, string body, bool isHtmlBody, SendMailAttachment attachment)
        {
            return Task.Run(() => this.SendEmail(from, to, subject, body, isHtmlBody, attachment));
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage, SendMailAttachment attachment, bool deleteAttachmentsAfterSend = false)
        {
            return Task.Run(() => this.SendEmail(Configuration.DefaultSenderAddress, email, subject, htmlMessage, true, attachment, deleteAttachmentsAfterSend));
        }

        public void SendEmail(string from, string to, string subject, string textBody, bool isHtmlBody, SendMailAttachment attachment, bool deleteAttachmentsAfterSend = false)
        {
            try
            {
                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                {
                    if (from.IsNullOrEmpty() || to.IsNullOrEmpty() || subject.IsNullOrEmpty() || textBody.IsNullOrEmpty())
                    {
                        // the addressee details are invalid.
#if ! DEBUG
                        throw new ArgumentException("You need to supply values for the parameters.");
#endif
                    }

                    MimeMessage message = new MimeMessage();
                    message.From.Add(new MailboxAddress(from));
                    message.To.Add(new MailboxAddress(to));
                    message.Subject = subject;

                    if (attachment != null)
                    {
                        if (! File.Exists(attachment.Path))
                        {
                            throw new FileNotFoundException(string.Format("The file at '{0}' could not found.", attachment.Path));
                        }

                        MimePart part = new MimePart(attachment.ContentType)
                        {
                            Content = new MimeContent(File.OpenRead(attachment.Path), ContentEncoding.Default),
                            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                            ContentTransferEncoding = ContentEncoding.Base64,
                            FileName = Path.GetFileName(attachment.Path)
                        };

                        Multipart multipart = new Multipart("mixed");

                        if (isHtmlBody == true)
                        {
                            // Add the same message as text incase the user does not want html emails to show.
                            BodyBuilder builder = new BodyBuilder();
                            builder.TextBody = textBody;
                            builder.HtmlBody = textBody;

                            multipart.Add(builder.ToMessageBody());
                        }
                        else
                        {
                            TextPart text = new TextPart("plain") { Text = textBody };
                            multipart.Add(text);
                        }

                        multipart.Add(part);

                        message.Body = multipart;
                    }
                    else
                    {
                        if (isHtmlBody == true)
                        {
                            // Add the same message as text incase the user does not want html emails to show.
                            BodyBuilder builder = new BodyBuilder();
                            builder.TextBody = textBody;
                            builder.HtmlBody = textBody;

                            message.Body = builder.ToMessageBody();
                        }
                        else
                        {
                            TextPart part = new TextPart("plain") { Text = textBody };
                            message.Body = part;
                        }
                    }

                    if (Configuration.Password.IsNullOrEmpty() == false)
                    {
                        using (SmtpClient client = new SmtpClient())
                        {
                            client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                            client.Connect(Configuration.MailHost, Configuration.Port, false);
                            client.Authenticate(Configuration.UserName, Configuration.Password);
                            client.Send(message);
                            client.Disconnect(true);
                        }
                    }

                    message = null;

                    if (attachment != null)
                    {
                        if (File.Exists(attachment.Path) && deleteAttachmentsAfterSend == true)
                        {
                            this.DeleteAttachment(attachment);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("The mail message could not be sent. See inner exception for details.", ex);
            }
        }

        private Task DeleteAttachmentAsync(SendMailAttachment attachment)
        {
            return Task.Run(() => this.DeleteAttachment(attachment));
        }

        private void DeleteAttachment(SendMailAttachment attachment)
        {
            if (File.Exists(attachment.Path))
            {
                // the file is in use until the smtp client lets it go.
                try
                {
                    File.Delete(attachment.Path);
                }
                catch (System.Exception)
                {
                    // do nothing. The file is locked by another process.
                }
            }
        }
    }
}
