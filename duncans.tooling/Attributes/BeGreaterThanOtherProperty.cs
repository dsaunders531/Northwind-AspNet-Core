// <copyright file="BeGreaterThanOtherProperty.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace duncans.shared.Attributes
{
    /// <summary>
    /// Check the offer id matches one in the database.
    /// </summary>
    public class BeGreaterThanOtherProperty : ValidationAttribute, IModelValidator
    {
        public Type Type { get; set; }

        public string OtherPropertyName { get; set; }

        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            IEnumerable<ModelValidationResult> result = Enumerable.Empty<ModelValidationResult>();

            switch (Type.ToString())
            {
                case "System.DateTime":
                    DateTime valueDt = Convert.ToDateTime(context.Model);
                    DateTime otherValueDt = Convert.ToDateTime(context.Container.GetPropertyValue(this.OtherPropertyName));
                    result = this.Validate(valueDt, otherValueDt);
                    break;
                case "System.Int32":
                    int valueInt = Convert.ToInt32(context.Model);
                    int otherValueInt = Convert.ToInt32(context.Container.GetPropertyValue(this.OtherPropertyName));
                    result = this.Validate(valueInt, otherValueInt);
                    break;
                default:
                    throw new NotImplementedException("The type you want to use has not been implemented in this attribute.");

                    // break;
            }

            return result;
        }

        private IEnumerable<ModelValidationResult> Validate(DateTime modelValue, DateTime otherValue)
        {
            IEnumerable<ModelValidationResult> result = Enumerable.Empty<ModelValidationResult>();

            if (modelValue <= otherValue)
            {
                // The test is less than the other value so...
                result = new List<ModelValidationResult>()
                {
                    new ModelValidationResult(
                        string.Empty,
                        string.Format("The date '{0}' is less than or equal to '{1}' which is not allowed.", modelValue.ToLongDateString(), otherValue.ToLongDateString()))
                };
            }

            return result;
        }

        private IEnumerable<ModelValidationResult> Validate(int modelValue, int otherValue)
        {
            IEnumerable<ModelValidationResult> result = Enumerable.Empty<ModelValidationResult>();

            if (modelValue <= otherValue)
            {
                // The test is less than the other value so...
                result = new List<ModelValidationResult>()
                {
                    new ModelValidationResult(
                        string.Empty,
                        string.Format("The number '{0}' is less than or equal to '{1}' which is not allowed.", modelValue.ToString(), otherValue.ToString()))
                };
            }

            return result;
        }
    }
}
