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
    public class ValidCustomerTypeAttribute : Attribute, IModelValidator
    {
        public bool IsRequired => true;

        public string ErrorMessage { get; set; } = "The customer type id does not exist";

        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            IEnumerable<ModelValidationResult> result = Enumerable.Empty<ModelValidationResult>();

            NorthwindDbContext dbContext = (NorthwindDbContext)context.ActionContext.HttpContext.RequestServices.GetService(typeof(NorthwindDbContext));
            
            IRepository<CustomerDemographicDbModel, long> repository = new CustomerDemographicRepository(dbContext);

            long? value = (long?)context.Model; 

            if (value == null)
            {
                result = new List<ModelValidationResult>() { new ModelValidationResult("", "A customer type id must be provided") };
            }
            else
            {
                CustomerDemographicDbModel model = repository.Fetch(value.Value);

                if (model == null)
                {
                    result = new List<ModelValidationResult>() { new ModelValidationResult("", ErrorMessage) };
                }
            }            

            return result;
        }
    }
}
