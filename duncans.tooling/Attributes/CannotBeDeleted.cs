// <copyright file="CannotBeDeleted.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using duncans.EF;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace duncans.shared.Attributes
{
    /// <summary>
    /// This validation attribute will only work when applied to a bool called DeletionForbidden and the underlying model implements 'IApiHistoricModel(long) and IDeleteable'.
    /// </summary>
    public class CannotBeDeleted : ValidationAttribute, IModelValidator
    {
        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            IEnumerable<ModelValidationResult> result = Enumerable.Empty<ModelValidationResult>();

            if (context.Model == null)
            {
                // failed. Must have a value.
                result = new List<ModelValidationResult>()
                    {
                        new ModelValidationResult(
                            string.Empty,
                            string.Format("{0} must have a value.", context.Container.GetDisplayName("DeletionForbidden")))
                    };
            }
            else
            {
                bool deletionForbidden = (bool)context.Model;
                IApiHistoricModel<long> model = (IApiHistoricModel<long>)context.Container;
                IDeleteable deleteable = (IDeleteable)context.Container;

                if ((model.Action == EFActionType.Delete && deletionForbidden == true) || (deleteable.Deleted == true && deletionForbidden == true))
                {
                    result = new List<ModelValidationResult>()
                    {
                        new ModelValidationResult(string.Empty, "This offer cannot be deleted because the deletion forbidden property has been set.")
                    };
                }
            }

            return result;
        }
    }
}
