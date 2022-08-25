using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Northwind.DAL;
using Northwind.DAL.Models;
using Northwind.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using tools.EF;

namespace Northwind.BLL.Validators
{
    /// <summary>
    /// ValidationAttribute to see if a category exists.
    /// </summary>
    public class ValidCategoryAttribute : Attribute, IModelValidator
    {
        public bool IsRequired => true;

        public string ErrorMessage { get; set; } = "The category id does not exist";

        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            IEnumerable<ModelValidationResult> result = Enumerable.Empty<ModelValidationResult>();

            // Dependancy injection does not work with attributes so manually wire up the database context.
            using (NorthwindContext dbContext = DAL.Startup.NorthwindContext)
            {
                IRepository<Category, int> categories = new CategoryRepository(dbContext);

                int? value = context.Model as int?; // get the value of supplier (the type must match the column type)

                if (value == null)
                {
                    // a supplier id must be supplied
                    result = new List<ModelValidationResult>() { new ModelValidationResult("", "A category id must be provided") };
                }
                else
                {
                    Category category = categories.Fetch(value.Value);

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
