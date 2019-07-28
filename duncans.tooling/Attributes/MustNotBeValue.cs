// <copyright file="MustNotBeValue.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace duncans.shared.Attributes
{
    public class MustNotBeValue : ValidationAttribute, IModelValidator
    {
        public Type Type { get; set; }

        public object MustNotValue { get; set; }

        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            IEnumerable<ModelValidationResult> result = Enumerable.Empty<ModelValidationResult>();

            object model = context.Model;

            if (model.GetType() == Type)
            {
                if (Convert.ChangeType(model, this.Type).ToString() == Convert.ChangeType(MustNotValue, this.Type).ToString())
                {
                    string thisDisplayName = context.Container.GetDisplayName(context.ModelMetadata.Name);

                    // The values match.
                    result = new List<ModelValidationResult>()
                    {
                        new ModelValidationResult(
                            string.Empty,
                            string.Format("The value for {0} must not be '{1}'", thisDisplayName, this.MustNotValue.ToString()))
                    };
                }
            }
            else
            {
                string thisDisplayName = context.Container.GetDisplayName(context.ModelMetadata.Name);

                // The types do not match
                result = new List<ModelValidationResult>()
                    {
                        new ModelValidationResult(
                            string.Empty,
                            string.Format("The type for {0} is not correct, it should be '{1}'", thisDisplayName, this.Type.ToString()))
                    };
            }

            return result;
        }
    }
}
