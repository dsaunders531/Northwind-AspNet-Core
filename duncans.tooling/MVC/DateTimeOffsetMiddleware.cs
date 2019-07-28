// <copyright file="DateTimeOffsetMiddleware.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace duncans.MVC
{
    public class DateTimeOffsetMiddleware
    {
        public DateTimeOffsetMiddleware(RequestDelegate next)
        {
            this.Next = next;
        }

        private RequestDelegate Next { get; set; }

        public async Task Invoke(HttpContext context)
        {
            const string offsetCookieName = "UserAgentTzOffset";
            byte[] junkValue = new byte[64];
            bool carryOn = false;
            int userAgentTzOffsetMins = 0;

            // See if a value has already been saved in session.
            if (context.Session.TryGetValue(offsetCookieName, out junkValue) == false)
            {
                int userOffsetMins = 0;

                // See if there is a header
                // Use script tz.js and CookieMonster.js to set the cookie
                if (context.Request.Cookies.ContainsKey(offsetCookieName))
                {
                    userOffsetMins = Convert.ToInt32(context.Request.Cookies[offsetCookieName]);
                    carryOn = true;
                }
                else if (context.Request.Headers.ContainsKey(offsetCookieName))
                {
                    userOffsetMins = Convert.ToInt32(context.Request.Headers[offsetCookieName]);
                    carryOn = true;
                }

                if (carryOn == true)
                {
                    // Save the offset in session
                    context.Session.SetInt32(offsetCookieName, userOffsetMins);
                    userAgentTzOffsetMins = userOffsetMins;
                }
            }
            else
            {
                carryOn = true;
                userAgentTzOffsetMins = context.Session.GetInt32(offsetCookieName).Value;
            }

            if (carryOn)
            {
                // Now loop through all the possible objects and alter all the data so it has the timezone offset.
                // Note: Route parameters and XML body will not be supported.

                // Query Parameters
                this.RemakeQueryString(context, userAgentTzOffsetMins);

                // x-form
                this.RemakeXForm(context, userAgentTzOffsetMins);

                // json
                this.RemakeJSON(context, userAgentTzOffsetMins);
            }

            try
            {
                await this.Next.Invoke(context);
            }
            catch (InvalidOperationException e)
            {
                // This err is caused by changing a status code in middleware when the response has already started.
                if ( e.Source != "Microsoft.AspNetCore.Server.Kestrel.Core")
                {
                    throw e;
                }
            }
        }

        private void RemakeJSON(HttpContext context, int userAgentTzOffsetMins)
        {
            if (context.Request.ContentType != null)
            {
                if (context.Request.ContentType.ToLower().Contains("json"))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        context.Request.Body.CopyTo(ms); // Note: context.Request.Body is a HttpRequestStream which is forward only and not writable. This means it needs to be replaced at the end of process even if nothing changes.

                        string json = ms.ToArray().FromBytes();

                        // Converting to an object is cumbersome and slow.
                        // Use regex to find date and date time strings.
                        // The risk of a pattern matching twice has been mitigated by including the ': "' at the start of a field value in the regex.
                        // Arrays of dates or times will not be supported.
                        string dateTimePattern = @":\s?\x22\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}"; // ': "yyyy-MM-dd HH:mm:ss'
                        string dateTimePatternJSON = @":\s?\x22\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}"; // ': "yyyy-MM-ddTHH:mm:ss'
                        string timePattern = @":\s?\x22\d{2}:\d{2}"; // ': "HH:mm' - the seconds are not important. Just the hour and minute offset needs changing.

                        List<KeyValuePair<string, string>> patterns = new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>("DateTime", dateTimePattern),
                            new KeyValuePair<string, string>("ISODateTime", dateTimePatternJSON),
                            new KeyValuePair<string, string>("Time", timePattern)
                        };

                        DateTime matchDateTime;

                        foreach (KeyValuePair<string, string> pattern in patterns)
                        {
                            Regex rg = new Regex(pattern.Value);
                            MatchCollection mc = rg.Matches(json);

                            if (mc.Count > 0)
                            {
                                foreach (Match item in mc)
                                {
                                    // remove the : " part of the string before trying to convert.
                                    string possibleDate = item.Value.Substring(item.Value.IndexOf("\"") + 1);

                                    if (DateTime.TryParse(possibleDate, out matchDateTime))
                                    {
                                        // only change things when there is a time element.
                                        if (matchDateTime.TimeOfDay.Ticks > 0)
                                        {
                                            if (pattern.Key == "Time")
                                            {
                                                // Keep the time formatting
                                                json = json.Replace(item.Value, string.Format(":\"{0}", matchDateTime.AddMinutes(userAgentTzOffsetMins * -1).ToString("HH:mm")));
                                            }
                                            else
                                            {
                                                json = json.Replace(item.Value, string.Format(":\"{0}", matchDateTime.AddMinutes(userAgentTzOffsetMins * -1).ToString("yyyy-MM-ddTHH:mm:ss")));
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // Save the object back to content event if there are no changes as its a readonce forward only stream.
                        context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(json));
                    }
                }
            }
        }

        private void RemakeXForm(HttpContext context, int userAgentTzOffsetMins)
        {
            if (context.Request.HasFormContentType && context.Request.Method != "GET")
            {
                // change the form value dates.
                AdaptableFormCollection form = new AdaptableFormCollection(context.Request.Form);
                ICollection<string> keys = form.Keys;

                foreach (string key in keys)
                {
                    StringValues formValue = form[key];

                    // test for date
                    DateTime formDate;

                    if (DateTime.TryParse(formValue, out formDate))
                    {
                        // Ignore 0 dates and datetimes without a time part.
                        if (formDate.TimeOfDay.Ticks > 0)
                        {
                            form.Remove(key);
                            form.Add(key, formDate.AddMinutes(userAgentTzOffsetMins * -1).ToXFormDateTimeString());
                        }
                    }
                }

                context.Request.Form = form;
            }
        }

        private void RemakeQueryString(HttpContext context, int userAgentTzOffsetMins)
        {
            if (context.Request.QueryString.HasValue)
            {
                string replacementQuery = "?";

                foreach (string key in context.Request.Query.Keys)
                {
                    string queryValue = context.Request.Query[key];
                    DateTime queryDateTime;

                    if (DateTime.TryParse(queryValue, out queryDateTime))
                    {
                        // Ignore 0 dates and datetimes without a time part.
                        if (queryDateTime.TimeOfDay.Ticks > 0)
                        {
                            // The parameter can be converted to a date.
                            queryValue = queryDateTime.AddMinutes(userAgentTzOffsetMins * -1).ToXFormDateTimeString();
                        }
                    }

                    if (replacementQuery.Length > 1)
                    {
                        replacementQuery += "&";
                    }

                    replacementQuery += key + "=" + queryValue.URLEncode();
                }

                context.Request.QueryString = new QueryString(replacementQuery);
            }
        }
    }
}
