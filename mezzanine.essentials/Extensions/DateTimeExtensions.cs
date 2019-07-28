// <copyright file="DateTimeExtensions.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace mezzanine
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Add the time zone offset in the users browser (using tz.js and DateTimeOffsetMiddleware) to the date time.
        /// </summary>
        /// <param name="value">The date you want to change.</param>
        /// <param name="session">The http context ISession where the value for the key named 'UserAgentTzOffset' is stored.</param>
        /// <returns></returns>
        public static DateTime ToUserTimeZone(this DateTime value, ISession session)
        {
            const string offsetCookieName = "UserAgentTzOffset";
            byte[] junkValue = new byte[64];

            if (value.Kind != DateTimeKind.Local)
            {
                value = new DateTime(value.Ticks, DateTimeKind.Local);
            }

            // See if the cookie value has been saved in session.
            if (session.TryGetValue(offsetCookieName, out junkValue))
            {
                int userAgentTzOffsetMins = session.GetInt32(offsetCookieName).Value;

                if (userAgentTzOffsetMins != 0)
                {
                    value = value.AddMinutes(userAgentTzOffsetMins);
                }
            }

            return value;
        }

        /// <summary>
        /// Add the required number of months to a date using the quantity of days in the following month as a basis.
        /// The built-in AddMonths did not produce the result I needed.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="months"></param>
        /// <returns></returns>
        /// <remarks>The standard .net library function uses the quantity of days in the current month.</remarks>
        public static DateTime AddMonthsDifferently(this DateTime value, int months)
        {
            if (months > 0)
            {
                for (int d = months; d > 0; d--)
                {
                    int nextMonth = value.Month == 12 ? 1 : value.Month + 1;
                    int nextYear = value.Month == 12 ? value.Year + 1 : value.Year;
                    int daysInNextMonth = (int)new DateTime(nextYear, nextMonth, 1).DaysInMonth();

                    value = value.AddDays(daysInNextMonth);

                    // This might happen for short months like February where the quantity of days pushed into the next month.
                    while (value.Month != nextMonth)
                    {
                        if (value.Month > nextMonth)
                        {
                            value = value.AddDays(-1);
                        }
                        else
                        {
                            value = value.AddDays(1);
                        }
                    }
                }
            }
            else if (months < 0)
            {
                for (int d = months; d < 0; d++)
                {
                    int lastMonth = value.Month == 1 ? 12 : value.Month - 1;
                    int lastYear = value.Month == 1 ? value.Year - 1 : value.Year;
                    int daysInLastMonth = (int)new DateTime(lastYear, lastMonth, 1).DaysInMonth();

                    value = value.AddDays(daysInLastMonth * -1);

                    // This might happen for short months like February where the quantity of days pushed into the next month.
                    while (value.Month != lastMonth)
                    {
                        if (value.Month > lastMonth)
                        {
                            value = value.AddDays(-1);
                        }
                        else
                        {
                            value = value.AddDays(1);
                        }
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Find the working date from now. Excluding weekends and specified holiday dates.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="daysFromNow"></param>
        /// <param name="holidayDates"></param>
        /// <returns></returns>
        public static DateTime NetworkDays(this DateTime value, int daysFromNow, List<DateTime> holidayDates)
        {
            DateTime result = value;

            while (daysFromNow > 0)
            {
                result = result.AddDays(1);
                bool isHoliday = false;

                if (holidayDates != null)
                {
                    isHoliday = holidayDates.Exists(d => d == result.Date);
                }

                if (!(result.DayOfWeek == DayOfWeek.Saturday || result.DayOfWeek == DayOfWeek.Sunday || isHoliday == true))
                {
                    daysFromNow--;
                }
            }

            return result.Date;
        }
       
        /// <summary>
        /// Find the last day of the month.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime LastOfMonth(this DateTime value)
        {
            DateTime result = value;
            int currentMonth = result.Month;

            result = new DateTime(result.Year, currentMonth, 28, 0, 0, 0, DateTimeKind.Utc); // create a new datetime value to increase speed

            while (result.Month == currentMonth)
            {
                result = result.AddDays(1);
            }

            return result.AddDays(-1);
        }

        public static DateTime FirstOfMonth(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        }

        /// <summary>
        /// Find the qauntity of days in a month.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal DaysInMonth(this DateTime value)
        {
            return value.LastOfMonth().Day;
        }

        /// <summary>
        /// Return the quantity of days in a year.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal DaysInYear(this DateTime value)
        {
            DateTime lastOfYear = new DateTime(value.Year, 12, 31, 0, 0, 0, DateTimeKind.Utc);
            return lastOfYear.DayOfYear;
        }

        /// <summary>
        /// The quantity of weeks in a month.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal WeeksInMonth(this DateTime value)
        {
            return value.DaysInMonth() / 7m;
        }

        /// <summary>
        /// Return a date time in the the ISO 8601 format.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToISODateString(this DateTime value)
        {
            if (value.Kind != DateTimeKind.Utc)
            {
                value = new DateTime(value.Ticks, DateTimeKind.Utc);
            }

            return value.ToString("yyyy-MM-ddTHH:mm:ssK");
        }

        public static string ToJsonDateTimeString(this DateTime value)
        {
            if (value.Kind != DateTimeKind.Utc)
            {
                value = new DateTime(value.Ticks, DateTimeKind.Utc);
            }

            return value.ToString("yyyy-MM-ddTHH:mm:ssZ");
        }

        public static string ToJsonDateString(this DateTime value)
        {
            if (value.Kind != DateTimeKind.Utc)
            {
                value = new DateTime(value.Ticks, DateTimeKind.Utc);
            }

            return value.Date.ToString("yyyy-MM-dd");
        }

        public static string ToXFormDateTimeString(this DateTime value)
        {
            if (value.Kind != DateTimeKind.Utc)
            {
                value = new DateTime(value.Ticks, DateTimeKind.Utc);
            }

            return value.ToString("dd/MM/yyyy HH:mm:ss");
        }

        public static string ToXFormDateString(this DateTime value)
        {
            if (value.Kind != DateTimeKind.Utc)
            {
                value = new DateTime(value.Ticks, DateTimeKind.Utc);
            }

            return value.ToString("dd/MM/yyyy");
        }

        /// <summary>
        /// The lowest system date.
        /// </summary>
        /// <returns></returns>
        public static DateTime DefaultLowDate()
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        }

        /// <summary>
        /// The highest system date.
        /// </summary>
        /// <returns></returns>
        public static DateTime DefaultHighDate()
        {
            return new DateTime(2100, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddTicks(-1);
        }

        public static string ToSqlDateTime(this DateTime value)
        {
            return value.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// Convert a windows datetime to a unix epoch number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToEpoch(this DateTime value)
        {
            // unix uses 1-Jan-1970 as base
            DateTime baseDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return (value - baseDate).Ticks / TimeSpan.TicksPerSecond;
        }

        /// <summary>
        /// Convert a unix epoch to a datetime.
        /// </summary>
        /// <param name="epochTime"></param>
        /// <returns></returns>
        public static DateTime FromEpoch(this double epochTime)
        {
            return FromEpoch(Convert.ToInt64(epochTime));
        }

        public static DateTime FromEpoch(this long epochTime)
        {
            // unix uses 1-Jan-1970 as base
            DateTime baseDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // Get the windows timespan as total seconds since 1-Jan-1970.
            TimeSpan totalSecondsSinceBase = new TimeSpan(epochTime * TimeSpan.TicksPerSecond);

            // Add the base and the seconds since together.
            return new DateTime(baseDate.Ticks + totalSecondsSinceBase.Ticks);
        }
    }
}