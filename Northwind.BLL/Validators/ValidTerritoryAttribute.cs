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
    public class ValidTerritoryAttribute : Attribute, IModelValidator
    {
        public bool IsRequired => true;

        public string ErrorMessage { get; set; } = "The territory id does not exist";

        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            IEnumerable<ModelValidationResult> result = Enumerable.Empty<ModelValidationResult>();

            // Dependancy injection does not work with attributes so manually wire up the database context.
            using (NorthwindContext dbContext = DAL.Startup.NorthwindContext)
            {
                IRepository<Territory, string> territories = new TerritoryRepository(dbContext);

                string value = context.Model as string;

                if (value == null)
                {
                    result = new List<ModelValidationResult>() { new ModelValidationResult("", "A territory id must be provided") };
                }
                else
                {
                    Territory territory = territories.Fetch(value);

                    if (territory == null)
                    {
                        result = new List<ModelValidationResult>() { new ModelValidationResult("", ErrorMessage) };
                    }
                }
            }

            return result;
        }
    }
}
