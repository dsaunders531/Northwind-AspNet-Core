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
    public class ValidEmployeeAttribute : Attribute, IModelValidator
    {
        public bool IsRequired => true;

        public string ErrorMessage { get; set; } = "The employee id does not exist";

        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            IEnumerable<ModelValidationResult> result = Enumerable.Empty<ModelValidationResult>();

            // Dependancy injection does not work with attributes so manually wire up the database context.
            using (NorthwindContext dbContext = DAL.Startup.NorthwindContext)
            {
                IRepository<Employee, int> repository = new EmployeeRepository(dbContext);

                int? value = context.Model as int?;

                if (value == null)
                {
                    result = new List<ModelValidationResult>() { new ModelValidationResult("", "An employee id must be provided") };
                }
                else
                {
                    Employee model = repository.Fetch(value.Value);

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
