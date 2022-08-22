using mezzanine.EF;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Northwind.DAL;
using Northwind.DAL.Models;
using Northwind.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Northwind.BLL.Validators
{
    public class ValidCustomerTypeAttribute : Attribute, IModelValidator
    {
        public bool IsRequired => true;

        public string ErrorMessage { get; set; } = "The customer type id does not exist";

        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            IEnumerable<ModelValidationResult> result = Enumerable.Empty<ModelValidationResult>();

            // Dependancy injection does not work with attributes so manually wire up the database context.
            using (NorthwindContext dbContext = DAL.Startup.NorthwindContext)
            {
                IRepository<CustomerDemographic, string> repository = new CustomerDemographicRepository(dbContext);

                string value = context.Model as string;

                if (value == null)
                {
                    result = new List<ModelValidationResult>() { new ModelValidationResult("", "A customer type id must be provided") };
                }
                else
                {
                    CustomerDemographic model = repository.Fetch(value);

                    if (model == null)
                    {
                        result = new List<ModelValidationResult>() { new ModelValidationResult("", ErrorMessage) };
                    }
                }
            }

            return result;
        }
    }
}
