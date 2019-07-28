// <copyright file="OneOfOtherPropertyMustBeValueWhenThisIsValue.cs" company="Duncan Saunders">
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
    /// Attribute to check if another property in a list matches a value on the specified value.
    /// Ie: see if one of many other boolean properties are true when this one is false.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class OneOfOtherPropertyMustBeValueWhenThisIsValue : ValidationAttribute, IModelValidator
    {
        public object ThisValue { get; set; }

        public object OtherPropertyValue { get; set; }

        public string[] OtherPropertyNames { get; set; }

        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            IEnumerable<ModelValidationResult> result = Enumerable.Empty<ModelValidationResult>();

            object modelValue = context.Model;

            if (modelValue.Equals(ThisValue))
            {
                // check one of the other values is set to OtherPropertyValue
                bool isOtherValue = false;

                foreach (string item in OtherPropertyNames)
                {
                    object otherPropertyActualValue = context.Container.GetPropertyValue(item);
                    isOtherValue = otherPropertyActualValue.Equals(OtherPropertyValue);
                    if (isOtherValue == true)
                    {
                        break;
                    }
                }

                if (isOtherValue == false)
                {
                    string thisDisplayName = context.Container.GetDisplayName(context.ModelMetadata.Name);
                    List<string> otherDisplayNames = new List<string>();

                    foreach (string item in OtherPropertyNames.ToList())
                    {
                        otherDisplayNames.Add(context.Container.GetDisplayName(item));
                    }

                    result = new List<ModelValidationResult>()
                            {
                                new ModelValidationResult(
                                                            context.ModelMetadata.Name,
                                                            string.Format(
                                                                "One of '{0}' must be '{1}' when {2} is '{3}'",
                                                                otherDisplayNames.ToCommaSeperatedString(),
                                                                OtherPropertyValue.ToString(),
                                                                thisDisplayName,
                                                                ThisValue.ToString()))
                    };
                }
            }

            return result;
        }
    }
}
