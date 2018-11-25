using mezzanine.Serialization;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;

namespace mezzanine
{
    /// <summary>
    /// Extensions for the string type.
    /// </summary>
    public static class StringExtentensions
    {
        /// <summary>
        /// Remove line breaks, tabs etc from a string.
        /// </summary>
        /// <param name="trim">Optionally, trim the white space from the string</param>
        /// <param name="value">The string you want to minify.</param>
        /// <returns>A flat string</returns>
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

        public static string UnMinify(this string value)
        {
            value = value.Replace("{", "{\r\n");
            value = value.Replace("}", "\r\n}");
            value = value.Replace(",\"", ",\r\n\"");
            return value.Trim();
        }

        /// <summary>
        /// Add an s to the end of text when the count is above 1.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="count"></param>
        /// <returns></returns>        
        public static string Pluralize(this string value, decimal count)
        {
            if (count != 1)
            {
                if (value.Last().ToString() !="y")
                {
                    value = value.Substring(0, value.Length - 1) + "ies";
                }
                else
                {
                    value += "s";
                }                
            }

            return value;
        }

        public static string Pluralize(this string value, long count)
        {
            return Pluralize(value, (decimal)count);
        }

        public static string Pluralize(this string value, int count)
        {
            return Pluralize(value, (decimal)count);
        }

        public static string Pluralize(this string value, short count)
        {
            return Pluralize(value, (decimal)count);
        }

        public static string Pluralize(this string value, byte count)
        {
            return Pluralize(value, (decimal)count);
        }

        public static string Pluralize(this string value, double count)
        {
            return Pluralize(value, (decimal)count);
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
        /// Get the log level from a string (ie: from the json config file)
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
        /// Convert a string to an array of bytes
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
        /// Convert an array of bytes to a string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FromBytes(this byte[] value)
        {
            string result = null;

            using (MemoryStream ms = new MemoryStream(value))
            {
                using (StreamReader sr = new StreamReader(ms))
                {
                    result = sr.ReadToEnd();
                }
            }

            return result;
        }

        /// <summary>
        /// Encode a string to Base64
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToBase64(this string value)
        {
            return Convert.ToBase64String(value.ToBytes());
        }

        /// <summary>
        /// Unencode a string from Base64
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FromBase64(this string value)
        {
            return Convert.FromBase64String(value).FromBytes();
        }

        /// <summary>
        /// Get the number of lines in a string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int Lines(this string value)
        {
            int result = 1;

            if (value.Contains("\r\n"))
            {
                result = value.Split("\r\n").Length;
            }
            else if (value.Contains("\r"))
            {
                result = value.Split("\r").Length;
            }
            else if (value.Contains("\n"))
            {
                result = value.Split("\n").Length;
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
        /// See if the string is erm... null or empty
        /// </summary>
        /// <param name="valuue"></param>
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
        /// See if a string contains any SQL injection.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ContainsSqlInjection(this string value)
        {
            bool result = false;
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

            return result;
        }
    }
}
