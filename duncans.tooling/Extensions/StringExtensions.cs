// <copyright file="StringExtensions.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using duncans.Serialization;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;

namespace duncans
{
    /// <summary>
    /// Extensions for the string type.
    /// </summary>
    public static class StringExtensions
    {
        public static string[] Split(this string value, string seperator)
        {
            return value.Split(seperator.ToCharArray());
        }

        /// <summary>
        /// Remove line breaks, tabs etc from a string.
        /// </summary>
        /// <param name="value">The string you want to minify.</param>
        /// /// <param name="trim">Optionally, trim the white space from the string.</param>
        /// <returns>A flat string.</returns>
        public static string Minify(this string value, bool trim = true)
        {
            value = value.Replace("\r", string.Empty); // Carriage Return
            value = value.Replace("\n", string.Empty); // New Line
            value = value.Replace(Environment.NewLine, string.Empty);

            value = value.Replace("\0", string.Empty); // Null
            value = value.Replace("\a", string.Empty); // Alert
            value = value.Replace("\b", string.Empty); // Backspace
            value = value.Replace("\f", string.Empty); // Form feed
            value = value.Replace("\t", string.Empty); // Horizontal tab
            value = value.Replace("\v", string.Empty); // Vertical tab

            if (trim == true)
            {
                value = value.Trim();
            }

            return value;
        }

        /// <summary>
        /// Count the quantity of times the search term appears in the string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public static int CountInstances(this string value, string searchTerm)
        {
            int result = 0;
            int instancePosition = value.IndexOf(searchTerm);

            if (instancePosition > -1)
            {
                result++;

                while ((instancePosition + 1) < value.Length)
                {
                    instancePosition = value.IndexOf(searchTerm, instancePosition + 1);

                    if (instancePosition > -1)
                    {
                        result++;
                    }
                    else if (instancePosition == -1)
                    {
                        break;
                    }
                }
            }

            return result;
        }

        public static string UnMinify(this string value)
        {
            value = value.Replace("{", "{\r\n");
            value = value.Replace("}", "\r\n}");
            value = value.Replace(",\"", ",\r\n\"");
            value = value.Replace("\n", "\r\n");
            value = value.Replace("\r", "\r\n");
            value = value.Replace("\r\n\r\n", "\r\n");
            return value.Trim();
        }

        /// <summary>
        /// Add an s to the end of text when the count is above 1.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="count"></param>
        /// <param name="singularWord"></param>
        /// <param name="pluralWord"></param>
        /// <returns></returns>
        public static string Pluralize(this string value, decimal count, string singularWord, string pluralWord)
        {
            ////// These are the rules:
            ////if (count != 1)
            ////{
            ////    value += "s";
            ////    // exceptions to the rule.
            ////    // Nouns ending in –s, -sh, -ch and –x, form their plurals by adding –es to the singular.
            ////    // Most nouns ending in –o, generally form their plurals by adding –es.
            ////    // Nouns ending in a consonant + -y, form their plurals by changing that –y into –i and adding –es.
            ////    // Most nouns ending in –f or –fe form their plurals by changing –f or –fe into v and adding –es.

            ////    // Can't keep a list of words as it would mean maintenance.
            ////}

            if (count != 1)
            {
                return pluralWord;
            }
            else
            {
                return singularWord;
            }
        }

        public static string Pluralize(this string value, long count, string singularWord, string pluralWord)
        {
            return Pluralize(value, (decimal)count, singularWord, pluralWord);
        }

        public static string Pluralize(this string value, int count, string singularWord, string pluralWord)
        {
            return Pluralize(value, (decimal)count, singularWord, pluralWord);
        }

        public static string Pluralize(this string value, short count, string singularWord, string pluralWord)
        {
            return Pluralize(value, (decimal)count, singularWord, pluralWord);
        }

        public static string Pluralize(this string value, byte count, string singularWord, string pluralWord)
        {
            return Pluralize(value, (decimal)count, singularWord, pluralWord);
        }

        public static string Pluralize(this string value, double count, string singularWord, string pluralWord)
        {
            return Pluralize(value, (decimal)count, singularWord, pluralWord);
        }

        /// <summary>
        /// Find the ordinal (1st, 2nd, 3rd) of the number and append it to the string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string Ordinal(this string value, decimal number)
        {
            string result = "th";

            if (number < 1)
            {
                // ordinals on minus numbers look strange.
                result = string.Empty;
            }
            else
            {
                // numbers ending 1 = st, 2 = nd, 3 = rd
                // number ending 11,12,13 = th
                string strNumber = number.ToString();

                int lastNumber = Convert.ToInt32(strNumber.Substring(strNumber.Length - 1, 1));

                switch (lastNumber)
                {
                    case 1:
                        result = "st";
                        break;
                    case 2:
                        result = "nd";
                        break;
                    case 3:
                        result = "rd";
                        break;
                    default:
                        result = "th";
                        break;
                }

                // add the rules for 11, 12, 13
                if (strNumber.Length >= 2)
                {
                    lastNumber = Convert.ToInt32(strNumber.Substring(strNumber.Length - 2, 2));

                    switch (lastNumber)
                    {
                        case 11:
                        case 12:
                        case 13:
                            result = "th";
                            break;
                        default:
                            break;
                    }
                }
            }

            return value + result;
        }

        public static string Ordinal(this string value, long number)
        {
            return Ordinal(value, (decimal)number);
        }

        public static string Ordinal(this string value, int number)
        {
            return Ordinal(value, (decimal)number);
        }

        public static string Ordinal(this string value, short number)
        {
            return Ordinal(value, (decimal)number);
        }

        public static string Ordinal(this string value, byte number)
        {
            return Ordinal(value, (decimal)number);
        }

        public static string Ordinal(this string value, double number)
        {
            return Ordinal(value, (decimal)number);
        }

        /// <summary>
        /// Get the log level from a string (ie: from the json config file).
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static LogLevel ToLogLevel(this string value)
        {
            LogLevel retLevel = Microsoft.Extensions.Logging.LogLevel.None;

            switch (value.ToLower())
            {
                case "trace":
                case "0":
                    retLevel = Microsoft.Extensions.Logging.LogLevel.Trace;
                    break;

                case "debug":
                case "1":
                    retLevel = Microsoft.Extensions.Logging.LogLevel.Debug;
                    break;

                case "information":
                case "info":
                case "2":
                    retLevel = Microsoft.Extensions.Logging.LogLevel.Information;
                    break;

                case "warning":
                case "3":
                    retLevel = Microsoft.Extensions.Logging.LogLevel.Warning;
                    break;

                case "error":
                case "4":
                    retLevel = Microsoft.Extensions.Logging.LogLevel.Error;
                    break;

                case "critical":
                case "5":
                    retLevel = Microsoft.Extensions.Logging.LogLevel.Error;
                    break;

                case "none":
                case "6":
                    retLevel = Microsoft.Extensions.Logging.LogLevel.None;
                    break;

                default:
                    retLevel = Microsoft.Extensions.Logging.LogLevel.None;
                    break;
            }

            return retLevel;
        }

        /// <summary>
        /// Convert a string JSON to an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T JSONConvert<T>(this string value)
        {
            T result = default(T);

            using (JSONSerializer js = new JSONSerializer())
            {
                result = js.Deserialize<T>(value);
            }

            return result;
        }

        /// <summary>
        /// Convert a string XML to an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T XMLConvert<T>(this string value)
        {
            T result = default(T);

            using (XMLSerializer xs = new XMLSerializer())
            {
                result = xs.Deserialize<T>(value);
            }

            return result;
        }

        /// <summary>
        /// Convert a string to an array of bytes.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this string value)
        {
            byte[] result = null;

            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(ms))
                {
                    sw.Write(value);
                    sw.Flush();
                }

                ms.Flush();

                result = ms.ToArray();
            }

            return result;
        }

        /// <summary>
        /// Convert an array of bytes to a string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FromBytes(this byte[] value)
        {
            string result = null;

            if (value != null)
            {
                using (MemoryStream ms = new MemoryStream(value))
                {
                    using (StreamReader sr = new StreamReader(ms))
                    {
                        result = sr.ReadToEnd();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Encode a string to Base64.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToBase64(this string value)
        {
            return Convert.ToBase64String(value.ToBytes());
        }

        /// <summary>
        /// Unencode a string from Base64.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FromBase64(this string value)
        {
            if (value == null)
            {
                return value;
            }
            else
            {
                return Convert.FromBase64String(value).FromBytes();
            }
        }

        /// <summary>
        /// Get the number of lines in a string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int Lines(this string value)
        {
            int result = 1;

            value = value.Replace("\r\n", "\n");
            value = value.Replace("\r", "\n");

            if (value.Contains("\n"))
            {
                result = value.Split(new char[] { Convert.ToChar("\n") }).Length;
            }

            return result;
        }

        public static string HTMLify(this string value, string enclosingElementName = @"", string enclosingElementClass = @"")
        {
            if (value.Contains("\r\n"))
            {
                value = value.Replace("\r\n", "</br>");
            }
            else if (value.Contains("\r"))
            {
                value = value.Replace("\r", "</br>");
            }
            else if (value.Contains("\n"))
            {
                value = value.Replace("\n", "</br>");
            }

            if (enclosingElementName != string.Empty)
            {
                if (enclosingElementClass != string.Empty)
                {
                    value = string.Format("<{0} class=\"{1}\">{2}</{0}>", enclosingElementName, enclosingElementClass, value);
                }
                else
                {
                    value = string.Format("<{0}>{1}</{0}>", enclosingElementName, value);
                }
            }

            value = value.Replace("</br></br>", "</br>");

            return value;
        }

        public static string ToTitleCase(this string value)
        {
            return ToTitleCase(value, System.Threading.Thread.CurrentThread.CurrentUICulture.TextInfo);
        }

        public static string ToTitleCase(this string value, CultureInfo culture)
        {
            return ToTitleCase(value, culture.TextInfo);
        }

        public static string ToTitleCase(this string value, TextInfo textInfo)
        {
            return textInfo.ToTitleCase(value);
        }

        public static string HTMLEncode(this string value)
        {
            return WebUtility.HtmlEncode(value);
        }

        public static string HTMLDecode(this string value)
        {
            return WebUtility.HtmlDecode(value);
        }

        public static string URLEncode(this string value)
        {
            return WebUtility.UrlEncode(value);
        }

        public static string URLDecode(this string value)
        {
            return WebUtility.UrlDecode(value);
        }

        /// <summary>
        /// See if the string is erm... null or empty.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Converts a string to upper camel case.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToCamelCase(this string value)
        {
            return value.ToTitleCase().Trim().Replace(" ", string.Empty);
        }

        /// <summary>
        /// Encrypt a string with the provider.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encryptionProvider"></param>
        /// <returns></returns>
        public static string Encrypt(this string value, IEncryptionProvider encryptionProvider)
        {
            return encryptionProvider.Encrypt(value);
        }

        /// <summary>
        /// Decrypt a string with the specified provider. Use the same provider you encypted the string with.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encryptionProvider"></param>
        /// <returns></returns>
        public static string Decrypt(this string value, IEncryptionProvider encryptionProvider)
        {
            return encryptionProvider.Decrypt(value);
        }

        /// <summary>
        /// Convert a Url to a Uri.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Uri ToUri(this string value)
        {
            string url = value;

            if (url.Last().ToString() == "/")
            {
                url = url.Substring(0, url.Length - 1);
            }

            url = url.Replace("://", "|");

            string[] parts = url.Split( new char[] { Convert.ToChar("|") });

            return new UriBuilder(parts[0], parts[1]).Uri;
        }

        /// <summary>
        /// Convert a null string to empty string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string NullToEmpty(this string value)
        {
            return value ?? string.Empty;
        }

        /// <summary>
        /// See if a string contains any SQL injection.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ContainsSqlInjection(this string value)
        {
            bool result = false;

            if (value.IsNullOrEmpty() == false)
            {
                string sqlText = value.Trim().Replace(" ", string.Empty).ToUpper();

                result = sqlText.Contains(";ALTER")
                         || sqlText.Contains(";CREATE")
                         || sqlText.Contains(";DROP")
                         || sqlText.Contains(";GRANT")
                         || sqlText.Contains(";SELECT")
                         || sqlText.Contains(";EXEC")
                         || sqlText.Contains(";DELETE")
                         || sqlText.Contains(";INSERT")
                         || sqlText.Contains(";UPDATE");
            }

            return result;
        }

        /// <summary>
        /// Test if the char is an upper case.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsUCase(this char value)
        {
            return char.IsUpper(value);
        }

        /// <summary>
        /// Take a camel case value and add spaces before the ucase letters.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string UnCamelCase(this string value)
        {
            string result = string.Empty;

            foreach (char item in value.ToArray())
            {
                if (item.IsUCase())
                {
                    result += " " + item;
                }
                else
                {
                    result += item;
                }
            }

            result = System.Threading.Thread.CurrentThread.CurrentUICulture.TextInfo.ToTitleCase(result).Trim();

            return result;
        }

        /// <summary>
        /// See if the HttpRequest header key name is one of the standard built in ones.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks>See https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers. </remarks>
        public static bool IsBuiltInHeaderKey(this string value)
        {
            bool result = false;

            switch (value.ToLower())
            {
                case "www-authenticate":
                case "authorization":
                case "proxy-authenticate":
                case "age":
                case "cache-control":
                case "clear-site-data":
                case "expires":
                case "pragma":
                case "warning":
                case "accept-ch":
                case "accept-ch-lifetime":
                case "early-data":
                case "content-dpr":
                case "dpr":
                case "save-data":
                case "viewport-width":
                case "width":
                case "last-modified":
                case "etag":
                case "if-match":
                case "if-none-match":
                case "if-modified-since":
                case "if-unmodified-since":
                case "vary":
                case "connection":
                case "keep-alive":
                case "accept":
                case "accept-charset":
                case "accept-encoding":
                case "accept-language":
                case "expect":
                case "max-forwards":
                case "cookies":
                case "set-cookie":
                case "cookie2":
                case "set-cookie2":
                case "access-control-allow-origin":
                case "access-control-allow-credentials":
                case "access-control-allow-headers":
                case "access-control-allow-methods":
                case "access-control-expose-headers":
                case "access-control-max-age":
                case "access-control-request-headers":
                case "access-control-request-method":
                case "origin":
                case "timing-allow-origin":
                case "x-permitted-cross-domain-policies":
                case "dnt":
                case "tk":
                case "content-disposition":
                case "content-length":
                case "content-type":
                case "content-encoding":
                case "content-language":
                case "content-location":
                case "forwarded":
                case "x-forwarded-for":
                case "x-forwarded-host":
                case "x-forwarded-proto":
                case "via":
                case "location":
                case "from":
                case "host":
                case "referer":
                case "referrer-policy":
                case "user-agent":
                case "allow":
                case "server":
                case "accept-ranges":
                case "range":
                case "if-range":
                case "content-range":
                case "cross-origin-resource-policy":
                case "content-security-policy":
                case "content-security-policy-report-only":
                case "expect-ct":
                case "feature-policy":
                case "public-key-pins":
                case "public-key-pins-report-only":
                case "strict-transport-security":
                case "upgrade-insecure-requests":
                case "x-content-type-options":
                case "x-download-options":
                case "x-frame-options":
                case "x-powered-by":
                case "x-xss-protection":
                case "last-event-id":
                case "nel":
                case "ping-from":
                case "ping-to":
                case "report-to":
                case "transfer-encoding":
                case "te":
                case "trailer":
                case "sec-websocket-key":
                case "sec-websocket-extensions":
                case "sec-websocket-accept":
                case "sec-websocket-protocol":
                case "sec-websocket-version":
                case "accept-push-policy":
                case "accept-signature":
                case "alt-svc":
                case "date":
                case "large-allocation":
                case "link":
                case "push-policy":
                case "retry-after":
                case "signature":
                case "signed-headers":
                case "server-timing":
                case "sourcemap":
                case "upgrade":
                case "x-dns-prefetch-control":
                case "x-firefox-spdy":
                case "x-pingback":
                case "x-requested-with":
                case "x-robots-tag":
                case "x-ua-compatible":
                    result = true;
                    break;

                default:
                    result = false;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Reduce the length of a string to the maximim value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string Truncate(this string value, int maxLength)
        {
            value = value.Trim();

            if (value.Length > maxLength)
            {
                value = value.Substring(0, maxLength);
            }

            return value;
        }
    }
}
