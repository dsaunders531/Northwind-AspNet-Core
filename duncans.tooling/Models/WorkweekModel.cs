// <copyright file="WorkweekModel.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace duncans.Models
{
    /// <summary>
    /// Define the working week days and times.
    /// Needed for calculating the earliest contract start date.
    /// </summary>
    [NotMapped]
    public class WorkWeekModel
    {
        /// <summary>
        /// Gets or sets the earliest start time for any transactions.
        /// </summary>
        [Required]
        [Display(Description = "The time when the business starts work.", Prompt = "", ShortName = "Opening Time", Name = "Opening Time")]
        [DataType(DataType.Time)]
        public TimeSpan OpenForBusiness { get; set; } = new TimeSpan(8, 0, 0);

        /// <summary>
        /// Gets or sets the latest time for any transactions.
        /// </summary>
        [Required]
        [Display(Description = "The time when the business stops work.", Prompt = "", ShortName = "Closing Time", Name = "Closing Time")]
        [DataType(DataType.Time)]
        public TimeSpan ClosedForBusiness { get; set; } = new TimeSpan(16, 0, 0);

        /// <summary>
        /// Gets or sets the first working day of the week.
        /// </summary>
        [Required]
        [Display(Description = "The first working day of the week.", Prompt = "", ShortName = "First working day of week", Name = "First working day of week")]
        [EnumDataType(typeof(DayOfWeek))]
        public DayOfWeek FirstWorkingDayOfWeek { get; set; } = DayOfWeek.Monday;

        /// <summary>
        /// Gets or sets which days are working days starting from the first day of the week.
        /// </summary>
        public bool[] WorkingDay { get; set; } = new bool[7];
    }
}
