using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Northwind.DAL;
using tools.EF;
using Northwind.DAL.Models;
using Northwind.DAL.Repositories;
using tools.Filters;

namespace Northwind.BLL.Validators
{
    public class ValidCustomerAttribute : Attribute, IModelValidator
    {
        public bool IsRequired => true;

        public string ErrorMessage { get; set; } = "The customer id does not exist";

        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            IEnumerable<ModelValidationResult> result = Enumerable.Empty<ModelValidationResult>();

            // Dependancy injection does not work with attributes so manually wire up the database context.
            using (NorthwindContext dbContext = DAL.Startup.NorthwindContext)
            {
                IRepository<Customer, string> repository = new CustomerRepository(dbContext);

                string value = context.Model as string;

                if (value == null)
                {
                    result = new List<ModelValidationResult>() { new ModelValidationResult("", "A customer id must be provided") };
                }
                else
                {
                    Customer model = repository.Fetch(value);

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
