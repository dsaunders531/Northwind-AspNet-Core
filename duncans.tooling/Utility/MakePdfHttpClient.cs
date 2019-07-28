// <copyright file="MakePdfHttpClient.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace duncans.tooling.Utility
{
    //// TODO change this so it uses HTMLRenderer.PDFshart. See the tests for it.
    //// No need to write files onto the filesystem.
    //// See InvoiceOrderAndTaskService.PublishInvoicesAsync.

    /// <summary>
    /// Web client which generates a PDF from a URL.
    /// Only works on Windows. Only works if Google Chrome is installed.
    /// </summary>
    public class MakePdfHttpClient : IDisposable
    {
        public MakePdfHttpClient()
        {
            this.RestClient = new RestSharp.RestClient();
        }

        private RestSharp.RestClient RestClient { get; set; }

        public void Dispose()
        {
            this.RestClient = null;
        }

        /// <summary>
        /// Generate a pdf from the url. Pass the context to pass the session cookies in the request so the required authentication is used.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="outputFilePath"></param>
        /// <param name="context"></param>
        /// <param name="cookies"></param>
        /// <param name="headers"></param>
        /// <returns>The path of the pdf.</returns>
        public string MakePdf(string url, string outputFilePath, HttpContext context, IRequestCookieCollection cookies, IHeaderDictionary headers)
        {
            RestRequest rq = new RestRequest(url, RestSharp.Method.GET);

            // Add the cookies and header from the context.
            if (context != null)
            {
                foreach (KeyValuePair<string, string> item in context.Request.Cookies)
                {
                    if (rq.Parameters.Where(t => t.Type == ParameterType.Cookie && t.Name == item.Key).Count() == 0)
                    {
                        rq.AddCookie(item.Key, item.Value);
                    }
                }

                IDictionary<string, StringValues> contextHeaders = context.Request.Headers;

                foreach (KeyValuePair<string, StringValues> item in contextHeaders)
                {
                    if (rq.Parameters.Where(t => t.Type == ParameterType.HttpHeader && t.Name == item.Key).Count() == 0)
                    {
                        if (item.Key.IsBuiltInHeaderKey() == false)
                        {
                            rq.AddHeader(item.Key, item.Value);
                        }
                    }
                }
            }

            if (cookies != null)
            {
                foreach (KeyValuePair<string, string> item in cookies)
                {
                    if (rq.Parameters.Where(t => t.Type == ParameterType.Cookie && t.Name == item.Key).Count() == 0)
                    {
                        rq.AddCookie(item.Key, item.Value);
                    }
                }
            }

            if (headers != null)
            {
                foreach (KeyValuePair<string, StringValues> item in headers)
                {
                    if (rq.Parameters.Where(t => t.Type == ParameterType.HttpHeader && t.Name == item.Key).Count() == 0)
                    {
                        if (item.Key.IsBuiltInHeaderKey() == false)
                        {
                            rq.AddHeader(item.Key, item.Value);
                        }
                    }
                }
            }

            IRestResponse resp = RestClient.Execute(rq);

            // Regardless of success or not, write out the content.
            string savedPath = System.Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            const string savedFile = "GenPdfFileTmp_";
            string tmpPath = CrossPlatform.BuildUniversalPath(new string[] { savedPath, savedFile + DateTime.UtcNow.TimeOfDay.TotalMilliseconds + ".html" });

            while (File.Exists(tmpPath))
            {
                System.Threading.Thread.Sleep(1); // wait and get another file name.
                tmpPath = CrossPlatform.BuildUniversalPath(new string[] { savedPath, savedFile + DateTime.UtcNow.TimeOfDay.TotalMilliseconds + ".html" });
            }

            // Loading xdoc does not work as there are c# chars which are not unscrambled.
            // use regex to find href="not http*" and src="not http*"
            // replace the text with link to url path.
            string content = this.PostProcessContent(resp.Content, url);

            File.WriteAllText(tmpPath, content);

            if (File.Exists(tmpPath))
            {
                outputFilePath = this.GeneratePDF(tmpPath, outputFilePath);

                try
                {
                    File.Delete(tmpPath);
                }
                catch (Exception)
                {
                    // ignore the error.
                    throw;
                }

                return outputFilePath;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Rewrite the links so they have full paths. This means the page displays properly in the next step.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private string PostProcessContent(string content, string url)
        {
            string[] patterns = new string[] { "href=\"(?!(http)).*", "src=\"(?!(http)).*" };
            Uri target = new Uri(url);

            foreach (string pattern in patterns)
            {
                Regex brokenLinks = new Regex(pattern);
                MatchCollection mc = brokenLinks.Matches(content);

                if (mc.Count > 0)
                {
                    foreach (Match item in mc)
                    {
                        string[] parts = item.Value.Split("\"");
                        string finalPart = string.Empty;

                        for (int i = 1; i < parts.Length; i++)
                        {
                            if (parts[i].IsNullOrEmpty() == false)
                            {
                                finalPart += parts[i] + "\"";
                            }
                        }

                        if (item.Value.EndsWith("\"") == false && finalPart.EndsWith("\"") == true)
                        {
                            finalPart = finalPart.Substring(0, finalPart.Length - 1);
                        }

                        if (item.Value.Contains("href") && (! item.Value.StartsWith("href=\"mailto:")))
                        {
                            content = content.Replace(
                                                    item.Value,
                                                    string.Format(
                                                        "{0}=\"{1}://{2}/{3}",
                                                        "href",
                                                        target.Scheme,
                                                        (target.Host.EndsWith("/") ? target.Host.Substring(0, target.Host.Length - 1) : target.Host) + ":" + target.Port,
                                                        finalPart));
                        }
                        else if (item.Value.Contains("src"))
                        {
                            content = content.Replace(
                                                    item.Value,
                                                    string.Format(
                                                        "{0}=\"{1}://{2}/{3}",
                                                        "src",
                                                        target.Scheme,
                                                        (target.Host.EndsWith("/") ? target.Host.Substring(0, target.Host.Length - 1) : target.Host) + ":" + target.Port,
                                                        finalPart));
                        }
                    }
                }
            }

            content = content.Replace("//", "/");

            return content;
        }

        /// <summary>
        /// Generate a pdf from a html page.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="outputPath"></param>
        /// <returns></returns>
        private string GeneratePDF(string url, string outputPath)
        {
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }

            string result = outputPath;

            if (CrossPlatform.IsWinOS())
            {
                // The aim is to make a command like this 'chrome --headless --disable-gpu --print-to-pdf={out path as c:\\temp\\thing.pdf} {full url}'

                // C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe
                const string appPath = "\\Google\\Chrome\\Application\\chrome.exe";

                string chromePath = System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + appPath;

                if (!File.Exists(chromePath))
                {
                    // try another path.
                    chromePath = System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + appPath;

                    if (!File.Exists(chromePath))
                    {
                        // Cannot find the exe in the ususal places.
                        throw new FileNotFoundException("The chrome executable cannot be found the usual places.");
                    }
                }

                string arguments = string.Empty;
                arguments = string.Format("/C \"{0}\" --headless --disable-gpu --run-all-compositor-stages-before-draw --print-to-pdf={1} --no-margins {2}", chromePath, outputPath, url);

                using (Process proc = System.Diagnostics.Process.Start("cmd.exe", arguments))
                {
                    proc.WaitForExit(6000); // wait upto 6 seconds for completion.

                    int counter = 0;

                    // Wait until the file has been written.
                    while (counter < 100)
                    {
                        if (File.Exists(outputPath))
                        {
                            break;
                        }

                        System.Threading.Thread.Sleep(100);
                        counter++;
                    }

                    proc.Close();

                    if (counter >= 100)
                    {
                        throw new TimeoutException("Creating a pdf took too long.");
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("PDF printing is not available on this operating system.");
            }

            return result;
        }
    }
}
