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
    /// Validation attribute to check the supplier exists.
    /// </summary>
    public class ValidSupplierAttribute : Attribute, IModelValidator
    {
        public bool IsRequired => true;

        public string ErrorMessage { get; set; } = "The supplier id does not exist";

        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            IEnumerable<ModelValidationResult> result = Enumerable.Empty<ModelValidationResult>();

            // Dependancy injection does not work with attributes so manually wire up the database context.
            using (NorthwindContext dbContext = DAL.Startup.NorthwindContext)
            {
                IRepository<Supplier, int> suppliers = new SupplierRepository(dbContext);

                int? value = context.Model as int?; // get the value of supplier (the type must match the column type)

                if (value == null)
                {
                    // a supplier id must be supplied
                    result = new List<ModelValidationResult>() { new ModelValidationResult("", "A supplier id must be provided") };
                }
                else
                {
                    Supplier supplier = suppliers.Fetch(value.Value);

                    if (supplier == null)
                    {
                        result = new List<ModelValidationResult>() { new ModelValidationResult("", ErrorMessage) };
                    }
                }
            }

            return result;
        }
    }
}
