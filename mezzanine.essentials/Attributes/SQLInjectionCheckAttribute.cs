using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Linq;

namespace mezzanine.Attributes
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
                result = new List<ModelValidationResult>() { new ModelValidationResult("", "Sql injection detected! Please remove invalid text to continue. ") };
            }

            return result;
        }
    }
}
