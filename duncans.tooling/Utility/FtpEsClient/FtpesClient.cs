// <copyright file="FtpesClient.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace duncans.Utility
{
    /// <summary>
    /// Client for FTPs or FTPes sites.
    /// </summary>
    public class FtpesClient
    {
        public FtpesClient(string Url, ICredentials credentials)
        {
            if (Url.EndsWith("/") == false)
            {
                Url += "/";
            }

            this.BaseUrl = Url.URLDecode();
            this.Credentials = credentials;
        }

        private string BaseUrl { get; set; }

        private ICredentials Credentials { get; set; }

        /// <summary>
        /// Get a directory listing including subdirectories of the base path.
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public List<FtpFileListModel> GetFiles(string relativePath = null)
        {
            List<FtpFileListModel> result = new List<FtpFileListModel>();

            Uri ftpUri = this.BaseUrl.ToUri();

            if (relativePath != null)
            {
                if (relativePath.EndsWith("/") == false)
                {
                    relativePath += "/";
                }

                ftpUri = new Uri(this.BaseUrl + relativePath);
            }

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUri);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.Credentials = this.Credentials.GetCredential(request.RequestUri, string.Empty);
            request.EnableSsl = true;
            request.UseBinary = true;

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == FtpStatusCode.OpeningData)
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        string content = sr.ReadToEnd();

                        // convert content to a list.
                        result.AddRange(this.ReadFtpFileListContent(content, relativePath));
                    }
                }
                else
                {
                    throw new WebException(string.Format("The ftp command to get files was not successful {0}.", response.StatusDescription));
                }
            }

            request = null;

            return result;
        }

        private List<FtpFileListModel> ReadFtpFileListContent(string content, string relativePath)
        {
            List<FtpFileListModel> result = new List<FtpFileListModel>();

            content = content.Replace("\r\n", "\n");
            content = content.Replace("\r", "\n");

            List<string> contents = content.Split(Convert.ToChar("\n")).ToList();

            foreach (string item in contents)
            {
                if (item.Length > 0)
                {
                    switch (item.First().ToString())
                    {
                        case "d":
                            // directory
                            // get related files too.
                            FtpFileListModel ftpFileList = new FtpFileListModel()
                            {
                                FileType = FtpFileType.directory,
                                Name = item.Split(Convert.ToChar(" ")).Last().Trim(),
                                Contents = new List<FtpFileListModel>(),
                                RelativePath = relativePath
                            };

                            ftpFileList.Contents.AddRange(this.GetFiles(relativePath + ftpFileList.Name));

                            result.Add(ftpFileList);

                            break;

                        case "l":
                            // These are links - ignore.
                            break;

                        case "-":
                            // file
                            result.Add(new FtpFileListModel()
                            {
                                FileType = FtpFileType.file,
                                Name = item.Split(Convert.ToChar(" ")).Last().Trim(),
                                Contents = null,
                                RelativePath = relativePath
                            });
                            break;

                        default:
                            break;
                    }
                }
            }

            return result;
        }
    }
}
