// <copyright file="ListExtensions.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace duncans
{
    public static class ListExtensions
    {
        /// <summary>
        /// Turn a list of string to a comma seperated list of strings.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="culture">Optional culture if you need output in a culture other than the running thread.</param>
        /// <returns></returns>
        public static string ToCommaSeperatedString(this List<string> list, CultureInfo culture = null)
        {
            string result = null;

            string listSeperator = culture != null ? culture.TextInfo.ListSeparator : System.Threading.Thread.CurrentThread.CurrentUICulture.TextInfo.ListSeparator;

            if (list != null)
            {
                if (list.Count > 0)
                {
                    foreach (string item in list)
                    {
                        if (item.Trim().Length > 0)
                        {
                            result += item.Trim() + listSeperator;
                        }
                    }

                    // tidy up the result
                    if (result.Last().ToString() == listSeperator)
                    {
                        result = result.Substring(0, result.Length - 1);
                    }
                }
                else
                {
                    return string.Empty;
                }
            }

            return result;
        }

        public static string ToCommaSeperatedString(this string[] list, CultureInfo culture = null)
        {
            string result = null;

            string listSeperator = culture != null ? culture.TextInfo.ListSeparator : System.Threading.Thread.CurrentThread.CurrentUICulture.TextInfo.ListSeparator;

            if (list != null)
            {
                if (list.Length > 0)
                {
                    foreach (string item in list)
                    {
                        if (item.Trim().Length > 0)
                        {
                            result += item.Trim() + listSeperator;
                        }
                    }

                    // tidy up the result
                    if (result.Last().ToString() == listSeperator)
                    {
                        result = result.Substring(0, result.Length - 1);
                    }
                }
                else
                {
                    return string.Empty;
                }
            }

            return result;
        }
    }
}
