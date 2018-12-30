using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Northwind.DAL;
using mezzanine.EF;
using Northwind.DAL.Models;
using Northwind.DAL.Repositories;

namespace Northwind.DAL.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class ValidTerritoryAttribute : Attribute, IModelValidator
    {
        public bool IsRequired => true;

        public string ErrorMessage { get; set; } = "The territory id does not exist";

        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            IEnumerable<ModelValidationResult> result = Enumerable.Empty<ModelValidationResult>();

            // Dependancy injection does not work with attributes so manually wire up the database context.
            using (NorthwindDbContext dbContext = DAL.Startup.NorthwindContext)
            {
                IRepository<TerritoryDbModel, int> territories = new TerritoryRepository(dbContext);

                int? value = (int?)context.Model; 

                if (value == null)
                {
                    result = new List<ModelValidationResult>() { new ModelValidationResult("", "A territory id must be provided") };
                }
                else
                {
                    TerritoryDbModel territory = territories.Fetch(value.Value);

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
