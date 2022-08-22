using tools.EF;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Northwind.DAL;
using Northwind.DAL.Models;
using Northwind.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Northwind.BLL.Validators
{
    public class ValidProductAttribute : Attribute, IModelValidator
    {
        public bool IsRequired => true;

        public string ErrorMessage { get; set; } = "The product id does not exist";

        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            IEnumerable<ModelValidationResult> result = Enumerable.Empty<ModelValidationResult>();

            // Dependancy injection does not work with attributes so manually wire up the database context.
            using (NorthwindContext dbContext = DAL.Startup.NorthwindContext)
            {
                IRepository<Product, int> products = new ProductRepository(dbContext);

                int? value = context.Model as int?;

                if (value == null)
                {
                    result = new List<ModelValidationResult>() { new ModelValidationResult("", "A product id must be provided") };
                }
                else
                {
                    Product category = products.Fetch(value.Value);

                    if (category == null)
                    {
                        result = new List<ModelValidationResult>() { new ModelValidationResult("", ErrorMessage) };
                    }
                }
            }

            return result;
        }
    }
}
