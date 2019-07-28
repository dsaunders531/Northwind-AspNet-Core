using duncans.EF;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Northwind.DAL.Models;
using Northwind.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Northwind.DAL.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class ValidProductAttribute : Attribute, IModelValidator
    {
        public bool IsRequired => true;

        public string ErrorMessage { get; set; } = "The product id does not exist";

        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            IEnumerable<ModelValidationResult> result = Enumerable.Empty<ModelValidationResult>();

            NorthwindDbContext dbContext = (NorthwindDbContext)context.ActionContext.HttpContext.RequestServices.GetService(typeof(NorthwindDbContext));
            
            IRepository<ProductDbModel, int> products = new ProductRepository(dbContext);

            int? value = context.Model as int?; 

            if (value == null)
            {
                result = new List<ModelValidationResult>() { new ModelValidationResult("", "A product id must be provided") };
            }
            else
            {
                ProductDbModel category = products.Fetch(value.Value);

                if (category == null)
                {
                    result = new List<ModelValidationResult>() { new ModelValidationResult("", ErrorMessage) };
                }
            }
           
            return result;
        }
    }
}
