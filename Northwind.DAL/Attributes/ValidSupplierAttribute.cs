﻿using duncans.EF;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Northwind.DAL.Models;
using Northwind.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Northwind.DAL.Attributes
{
    /// <summary>
    /// Validation attribute to check the supplier exists.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class ValidSupplierAttribute : Attribute, IModelValidator
    {
        public bool IsRequired => true;

        public string ErrorMessage { get; set; } = "The supplier id does not exist";

        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            IEnumerable<ModelValidationResult> result = Enumerable.Empty<ModelValidationResult>();

            NorthwindDbContext dbContext = (NorthwindDbContext)context.ActionContext.HttpContext.RequestServices.GetService(typeof(NorthwindDbContext));
           
            IRepository<SupplierDbModel, int> suppliers = new SupplierRepository(dbContext);

            int? value = context.Model as int?; // get the value of supplier (the type must match the column type)

            if (value == null)
            {
                // a supplier id must be supplied
                result = new List<ModelValidationResult>() { new ModelValidationResult("", "A supplier id must be provided") };
            }
            else
            {
                SupplierDbModel supplier = suppliers.Fetch(value.Value);

                if (supplier == null)
                {
                    result = new List<ModelValidationResult>() { new ModelValidationResult("", ErrorMessage) };
                }
            }
            

            return result;
        }
    }
}