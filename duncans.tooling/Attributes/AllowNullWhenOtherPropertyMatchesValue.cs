// <copyright file="AllowNullWhenOtherPropertyMatchesValue.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace duncans.shared.Attributes
{
    /// <summary>
    /// Attribute to allow a property to be null if another property has a specific value.
    /// </summary>
    public class AllowNullWhenOtherPropertyMatchesValue : ValidationAttribute, IModelValidator
    {
        public string OtherPropertyName { get; set; }

        public object AllowNullOnValue { get; set; }

        /// <summary>
        /// Allow the model value to be null when the other property has the allowed value.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            IEnumerable<ModelValidationResult> result = Enumerable.Empty<ModelValidationResult>();

            if (context.Model == null)
            {
                object propertyValue = context.Container.GetPropertyValue(this.OtherPropertyName);

                if (!propertyValue.Equals(AllowNullOnValue))
                {
                    string thisDisplayName = context.Container.GetDisplayName(context.ModelMetadata.Name);
                    string otherDisplayName = context.Container.GetDisplayName(this.OtherPropertyName);

                    // failed. the other property does not match the required value.
                    result = new List<ModelValidationResult>()
                    {
                        new ModelValidationResult(
                            string.Empty,
                            string.Format(
                                "{0} must have a value when {1} is not equal to {2}.",
                                thisDisplayName,
                                otherDisplayName,
                                this.AllowNullOnValue.ToString()))
                    };
                }
            }

            return result;
        }
    }
}
