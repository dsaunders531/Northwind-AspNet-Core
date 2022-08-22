using mezzanine.Utility;
using Microsoft.EntityFrameworkCore;
using Northwind.DAL;
using Northwind.DAL.Models;
using System.Collections.Generic;
using Xunit;

namespace Northwind.Tests
{
    public class EFContextTests
    {
        private const string ConnectionString = "Server=LAPTOP10\\SQLEXPRESS;Database=Northwind;Trusted_Connection=true;MultipleActiveResultSets=true";

        /// <summary>
        /// Create the nortwind context.
        /// </summary>
        /// <returns></returns>
        public NorthwindContext NorthwindContext()
        {
            NorthwindContext result = null;

            DbContextOptionsBuilder<NorthwindContext> optionsBuilder = new DbContextOptionsBuilder<NorthwindContext>()
                                                                            .UseSqlServer(ConnectionString);
            result = new NorthwindContext(optionsBuilder.Options);

            optionsBuilder = null;

            return result;
        }

        [Fact]
        public void RunStoredProcComplexTypeNoParameters()
        {
            using (NorthwindContext context = NorthwindContext())
            {
                List<MostExpensiveProduct> mostExpensiveProducts = context.TenMostExpensiveProducts();
                Assert.True(mostExpensiveProducts.Count == 10, "Ten products were not found");
            }
        }

        [Fact]
        public void RunStoredProcComplexTypeWithParameters()
        {
            using (NorthwindContext context = NorthwindContext())
            {
                List<SalesByCategory> salesByCategory = context.SalesByCategory("Beverages", 1998);

                using (MSSQLDbClient client = new MSSQLDbClient(ConnectionString))
                {
                    List<SalesByCategory> salesByCategoryDirect = client.Fill<List<SalesByCategory>>("exec [SalesByCategory] 'Beverages', 1998");
                    Assert.True(salesByCategory.Count == salesByCategoryDirect.Count, "Sales by category returned incorrect results.");
                }
            }
        }
    }
}
