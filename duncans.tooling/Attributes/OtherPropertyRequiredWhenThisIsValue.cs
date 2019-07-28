// <copyright file="OtherPropertyRequiredWhenThisIsValue.cs" company="Duncan Saunders">
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
    /// Validator to use when another property must be populated if this one matches a sepecified value.
    /// </summary>
    public class OtherPropertyRequiredWhenThisIsValue : ValidationAttribute, IModelValidator
    {
        public string OtherPropertyName { get; set; }

        public object MatchValue { get; set; }

        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            IEnumerable<ModelValidationResult> result = Enumerable.Empty<ModelValidationResult>();

            object value = context.Model;

            if (value.ToString() == this.MatchValue.ToString())
            {
                // The desired value has been set. Check the other property value.
                object otherValue = context.Container.GetPropertyValue(this.OtherPropertyName);
                Type otherType = context.Container.GetPropertyType(this.OtherPropertyName);

                object defaultOtherValue = null;

                try
                {
                    defaultOtherValue = Activator.CreateInstance(otherType);
                }
                catch (Exception)
                {
                    // do nothing, the default value will be null
                }

                if (otherValue == null || otherValue.Equals(defaultOtherValue))
                {
                    string thisDisplayName = context.Container.GetDisplayName(context.ModelMetadata.Name);
                    string otherDisplayName = context.Container.GetDisplayName(this.OtherPropertyName);

                    // The other value is null or default so its failed the test.
                    result = new List<ModelValidationResult>()
                            {
                                new ModelValidationResult(
                                                            context.ModelMetadata.Name,
                                                            string.Format(
                                                                        "{0} must have a value when {1} is {2}.",
                                                                        otherDisplayName,
                                                                        thisDisplayName,
                                                                        value.ToString()))
                            };
                }
            }

            return result;
        }
    }
}
