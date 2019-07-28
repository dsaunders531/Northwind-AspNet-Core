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
    public class ValidOrderAttribute : Attribute, IModelValidator
    {
        public bool IsRequired => true;

        public string ErrorMessage { get; set; } = "The order id does not exist";

        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            IEnumerable<ModelValidationResult> result = Enumerable.Empty<ModelValidationResult>();

            NorthwindDbContext dbContext = (NorthwindDbContext)context.ActionContext.HttpContext.RequestServices.GetService(typeof(NorthwindDbContext));
            
            IRepository<OrderDbModel, int> repository = new OrderRepository(dbContext);

            int? value = context.Model as int?; 

            if (value == null)
            {
    
                result = new List<ModelValidationResult>() { new ModelValidationResult("", "A order id must be provided") };
            }
            else
            {
                OrderDbModel model = repository.Fetch(value.Value);

                if (model == null)
                {
                    result = new List<ModelValidationResult>() { new ModelValidationResult("", ErrorMessage) };
                }
            }
      
            return result;
        }
    }
}
