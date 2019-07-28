// <copyright file="ExceptionExtensions.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System;

namespace duncans
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// A longer message which is the type and the message plus inner exception details.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string LongMessage(this Exception e)
        {
            string result = string.Format("{0} {1}", e.GetType().ToString(), e.Message);

            // Add 1 level of inner exception. Any more levels and the message would be too long.
            if (e.InnerException != null)
            {
                result += string.Format("Inner exception: {0} {1}", e.InnerException.GetType().ToString(), e.InnerException.Message);
            }

            return result;
        }
    }
}
