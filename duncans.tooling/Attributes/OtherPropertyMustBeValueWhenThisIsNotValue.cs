// <copyright file="OtherPropertyMustBeValueWhenThisIsNotValue.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using IModelValidator = Microsoft.AspNetCore.Mvc.ModelBinding.Validation.IModelValidator;

namespace duncans.shared.Attributes
{
    /// <summary>
    /// Attribute to check if another property matches a value on the specified value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class OtherPropertyMustBeValueWhenThisIsNotValue : ValidationAttribute, IModelValidator
    {
        public object ThisValue { get; set; }

        public object OtherPropertyValue { get; set; }

        public string OtherPropertyName { get; set; }

        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            IEnumerable<ModelValidationResult> result = Enumerable.Empty<ModelValidationResult>();

            object modelValue = context.Model;

            if (modelValue != null)
            {
                if (! modelValue.Equals(ThisValue))
                {
                    // check one of the other values is set to OtherPropertyValue
                    bool isOtherValue = false;

                    object otherPropertyActualValue = context.Container.GetPropertyValue(this.OtherPropertyName);
                    isOtherValue = otherPropertyActualValue.Equals(OtherPropertyValue);

                    if (isOtherValue == false)
                    {
                        string thisDisplayName = context.Container.GetDisplayName(context.ModelMetadata.Name);
                        string otherDisplayName = context.Container.GetDisplayName(this.OtherPropertyName);

                        result = new List<ModelValidationResult>()
                        {
                                new ModelValidationResult(
                                                            context.ModelMetadata.Name,
                                                            string.Format(
                                                                "{0} must be '{1}' when {2} is not '{3}'",
                                                                otherDisplayName,
                                                                OtherPropertyValue.ToString(),
                                                                thisDisplayName,
                                                                ThisValue.ToString().IsNullOrEmpty() ? "Empty String" : ThisValue.ToString()))
                        };
                    }
                }
            }

            return result;
        }
    }
}
