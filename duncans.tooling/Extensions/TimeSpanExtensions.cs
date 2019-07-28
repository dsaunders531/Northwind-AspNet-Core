// <copyright file="TimeSpanExtensions.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System;

namespace duncans
{
    public static class TimeSpanExtensions
    {
        public static TimeSpan WithServerUTCOffset(this TimeSpan value)
        {
            DateTime testDate = DateTime.UtcNow;
            int minutesOffset = (int)TimeZoneInfo.Local.GetUtcOffset(testDate).TotalMinutes;

            return new TimeSpan(0, (int)value.TotalMinutes + minutesOffset, 0);
        }
    }
}
