// <copyright file="HttpRequestExtensions.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Globalization;
using System.Linq;

namespace duncans
{
    public static class HttpRequestExtensions
    {
        public static string PathAndQuery(this HttpRequest request) => request.QueryString.HasValue
                                                                   ? $"{request.Path}{request.QueryString}"
                                                                   : request.Path.ToString();

        /// <summary>
        /// Return the UserCulture of the httpRequest
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static CultureInfo Culture(this HttpRequest request)
        {
            CultureInfo result = System.Threading.Thread.CurrentThread.CurrentUICulture;

            StringValues headerDataStringValues = StringValues.Empty;

            string firstAcceptLanguage = string.Empty;

            char[] mainSplit = new char[] { ';' }; // The header accept language needs splitting twice
            char[] secondSplit = new char[] { ',' };

            try
            {
                if (request.Headers?.TryGetValue("Accept-Language", out headerDataStringValues) == true)
                {
                    firstAcceptLanguage = headerDataStringValues.FirstOrDefault();
                    firstAcceptLanguage = firstAcceptLanguage.Split(mainSplit)[0];
                    firstAcceptLanguage = firstAcceptLanguage.Split(secondSplit)[0];

                    if (firstAcceptLanguage != string.Empty || firstAcceptLanguage != null)
                    {
                        result = new CultureInfo(firstAcceptLanguage);
                    }
                }
            }
            catch
            {
                // falback to a default
                result = System.Threading.Thread.CurrentThread.CurrentUICulture;
            }

            return result;
        }

        /// <summary>
        /// Return the region for the httpRequest based on geolocation or request culture.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>If you want to seperate the location from the culture. You will need to use a reverse geolocation service and use it to set the region info.</remarks>
        public static RegionInfo Region(this HttpRequest request)
        {
            RegionInfo result = request.Culture().GetRegions().FirstOrDefault();
            return result;
        }
    }
}
