// <copyright file="WithinDateRangeAttribute.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;

namespace duncans.Filters
{
    /// <summary>
    /// Attribute to check that a date is within the default low and high dates.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class WithinDateRangeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime? dateTime = null;

                try
                {
                    dateTime = (DateTime)value;
                }
                catch (InvalidCastException)
                {
                    DateTime convertedDateTime;

                    if (DateTime.TryParse(value.ToString().URLDecode(), out convertedDateTime))
                    {
                        dateTime = convertedDateTime;
                    }
                }

                if (dateTime.HasValue)
                {
                    if (dateTime < DateTimeExtensions.DefaultLowDate())
                    {
                        return new ValidationResult(
                                            string.Format(
                                                        "The date must be between {0} and {1}.",
                                                        DateTimeExtensions.DefaultLowDate().ToShortDateString(),
                                                        DateTimeExtensions.DefaultHighDate().ToShortDateString()));
                    }
                    else if (dateTime > DateTimeExtensions.DefaultHighDate())
                    {
                        return new ValidationResult(
                                            string.Format(
                                                        "The date must be between {0} and {1}.",
                                                        DateTimeExtensions.DefaultLowDate().ToShortDateString(),
                                                        DateTimeExtensions.DefaultHighDate().ToShortDateString()));
                    }
                    else
                    {
                        return ValidationResult.Success;
                    }
                }
                else
                {
                    // When the value is null assume success. This covers cases of nullable types.
                    return ValidationResult.Success;
                }
            }
            else
            {
                // When the value is null assume success. This covers cases of nullable types.
                return ValidationResult.Success;
            }
        }
    }
}
