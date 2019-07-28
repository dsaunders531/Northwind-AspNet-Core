// <copyright file="HttpResponseExtensions.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using duncans.RestClient;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;

namespace duncans
{
    public static class HttpResponseExtensions
    {
        public static void CreateCookie(this HttpResponse me, string key, string value, int lifeTimeDays)
        {
            me.Cookies.Append(key, value, new CookieOptions() { IsEssential = true, Expires = DateTime.UtcNow.AddDays(lifeTimeDays) });
        }

        public static void DeleteCookie(this HttpResponse me, string key)
        {
            me.Cookies.Delete(key);
        }

        /// <summary>
        /// Return text (en-GB) for the status code. The request is used to provide extra information in the response.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static string StatusCodeMeaning(this HttpResponse response)
        {
            return response.StatusCode.ToStatusCodeMeaning(response.HttpContext?.Request ?? default(HttpRequest));
        }

        /// <summary>
        /// Add response body.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="body"></param>
        public static void AddBody(this HttpResponse response, string body)
        {
            using (MemoryStream ms = new MemoryStream(body.ToBytes()))
            {
                ms.WriteTo(response.Body);
            }
        }

        /// <summary>
        /// Add response body.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="body"></param>
        /// <param name="statusCode"></param>
        public static void AddBody(this HttpResponse response, string body, int statusCode)
        {
            response.StatusCode = statusCode;
            response.AddBody(body);
        }

        /// <summary>
        /// Add response body containing useful error information.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="ex"></param>
        public static void AddBody(this HttpResponse response, Exception ex)
        {
            if (ex != null)
            {
                string errString = string.Format("An exception happened processing your request on {0}. ", ex.TargetSite?.ToString());

                Exception mainEx = ex;

                while (ex != null)
                {
                    errString += string.Format("'{0}' was thrown with '{1}'", ex.GetType().ToString(), ex.Message);

                    if (errString.Last() != Convert.ToChar("."))
                    {
                        errString += ".";
                    }

                    ex = ex.InnerException;
                }

                errString = new InternalServerError() { ExceptionType = mainEx.GetType().ToString(), Message = errString, Trace = mainEx.StackTrace }.ToJson();

                response.StatusCode = StatusCodes.Status500InternalServerError;
                response.AddBody(errString);
            }
        }
    }
}
