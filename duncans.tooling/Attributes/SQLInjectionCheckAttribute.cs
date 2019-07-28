// <copyright file="SQLInjectionCheckAttribute.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace duncans.Filters
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class SqlInjectionCheckAttribute : ValidationAttribute, IModelValidator
    {
        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            IEnumerable<ModelValidationResult> result = Enumerable.Empty<ModelValidationResult>();

            string value = context.Model as string;

            if (value.ContainsSqlInjection() == true)
            {
                result = new List<ModelValidationResult>() { new ModelValidationResult(string.Empty, "Sql injection detected! Please remove invalid text to continue. ") };
            }

            return result;
        }
    }
}
