// <copyright file="SendMailAttachment.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.StaticFiles;
using MimeKit;
using System.IO;

namespace duncans.Utility
{
    /// <summary>
    /// Class to define an email attachment
    /// </summary>
    public class SendMailAttachment
    {
        public SendMailAttachment() { }

        public SendMailAttachment(string path)
        {
            this.Path = path;

            // try to get the content type from the file at the path.
            if (System.IO.File.Exists(path))
            {
                FileExtensionContentTypeProvider prov = new FileExtensionContentTypeProvider();

                string fileContentType;

                if (! prov.TryGetContentType(path, out fileContentType))
                {
                    fileContentType = "application/octet-stream"; // default.
                }

                if (fileContentType.Contains("/"))
                {
                    string[] parts = fileContentType.Split("/");

                    this.ContentType = new ContentType(parts[0], parts[1]);
                }
            }
            else
            {
                throw new FileNotFoundException(string.Format("The file at '{0}' was not found.", path));
            }
        }

        public string Path { get; set; }

        public ContentType ContentType { get; set; }
    }
}
