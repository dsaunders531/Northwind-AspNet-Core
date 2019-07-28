// <copyright file="DoubleExtensions.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System;
using System.Linq;

namespace duncans
{
    public static class DoubleExtensions
    {
        /// <summary>
        /// Round a number down by ignoring the additional decimals.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decimalPlaces"></param>
        /// <returns></returns>
        /// <remarks>eg: 1.239 to 2 dp is 1.23. 1.231 to 2dp is 1.23.</remarks>
        public static double IgnoreDecimal(this double value, short decimalPlaces)
        {
            double multiplyer = Math.Pow(10, decimalPlaces);
            double result = (long)(value * multiplyer);
            result = result / multiplyer;
            return result;
        }

        /// <summary>
        /// Round a number down by ignoring the additional decimals.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decimalPlaces"></param>
        /// <returns></returns>
        /// <remarks>eg: 1.239 to 2 dp is 1.23. 1.231 to 2dp is 1.23.</remarks>
        public static decimal IgnoreDecimal(this decimal value, short decimalPlaces)
        {
            decimal multiplyer = (decimal)Math.Pow(10, decimalPlaces);
            decimal result = (long)(value * multiplyer);
            result = result / multiplyer;
            return result;
        }

        /// <summary>
        /// Round down a number to the specified decimal places.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decimalPlaces"></param>
        /// <returns></returns>
        public static double RoundDown(this double value, short decimalPlaces)
        {
            return value.IgnoreDecimal(decimalPlaces);
        }

        /// <summary>
        /// Round down a number to the specified decimal places.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decimalPlaces"></param>
        /// <returns></returns>
        public static decimal RoundDown(this decimal value, short decimalPlaces)
        {
            return value.IgnoreDecimal(decimalPlaces);
        }

        /// <summary>
        /// Round up a number to the specified decimal places.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decimalPlaces"></param>
        /// <returns></returns>
        /// <remarks>
        /// The built-in rounding function Math.Round() produces wierd results.
        /// It does really...
        /// </remarks>
        public static decimal Round(this decimal value, short decimalPlaces)
        {
            return (decimal)((double)value).Round(decimalPlaces);
        }

        /// <summary>
        /// Round up a number to the specified decimal places. At 5 or above the value is rounded up.
        /// Below 5, the number is rounded down.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decimalPlaces"></param>
        /// <returns></returns>
        /// <remarks>
        /// The built-in rounding function Math.Round() produces wierd results.
        /// It does really...
        /// </remarks>
        public static double Round(this double value, short decimalPlaces)
        {
            double multiplyer = Math.Pow(10, decimalPlaces);

            long baseValue = (long)(value * multiplyer);

            double lastdecimal = ((value * multiplyer) - (double)baseValue) * 10;

            // This is where its wierd:
            //    at 5 we round up.
            //    at 3 we round down.
            //    at 4 we round down
            //    at 4.1 we round down
            //    above 4.5 round up
            if ((byte)lastdecimal == 4)
            {
                // do the additional midoint work
                // look at the decimal parts by turning it into a string
                string[] roundParts = lastdecimal.ToString().Split(new char[] { '.' });

                lastdecimal = lastdecimal.RoundDown(0);

                if (roundParts.Length >= 2)
                {
                    // note at this point its only at above 5
                    byte first = Convert.ToByte(roundParts[1].Substring(0, 1));

                    if (first > 5)
                    {
                        lastdecimal = lastdecimal + 1;
                    }
                }
            }

            if (lastdecimal >= 5)
            {
                baseValue = baseValue + 1;
            }

            return (double)baseValue / multiplyer;
        }

        /// <summary>
        /// Round up a number.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decimalPlaces"></param>
        /// <returns></returns>
        public static decimal RoundUp(this decimal value, short decimalPlaces)
        {
            return (decimal)((double)value).RoundUp(decimalPlaces);
        }

        /// <summary>
        /// Round up a number.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decimalPlaces"></param>
        /// <returns></returns>
        public static double RoundUp(this double value, short decimalPlaces)
        {
            double multiplyer = Math.Pow(10, decimalPlaces);

            long baseValue = (long)(value * multiplyer);

            double lastdecimal = ((value * multiplyer) - (double)baseValue) * 10;

            if (lastdecimal > 0)
            {
                baseValue = baseValue + 1;
            }

            return (double)baseValue / multiplyer;
        }
    }
}
