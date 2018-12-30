using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using mezzanine.DbClient;
using Northwind.BLL.Models;
using Northwind.DAL;
using Microsoft.EntityFrameworkCore;
using Northwind.DAL.Models;

namespace Northwind.Tests
{
    public class EFContextTests
    {
        private const string ConnectionString = "Server=LAPTOP10\\SQLEXPRESS;Database=Northwind;Trusted_Connection=true;MultipleActiveResultSets=true";

        /// <summary>
        /// Create the nortwind context.
        /// </summary>
        /// <returns></returns>
        public NorthwindDbContext NorthwindContext()
        {
            NorthwindDbContext result = null;

            DbContextOptionsBuilder<NorthwindDbContext> optionsBuilder = new DbContextOptionsBuilder<NorthwindDbContext>()
                                                                            .UseSqlServer(ConnectionString);
            result = new NorthwindDbContext(optionsBuilder.Options);

            optionsBuilder = null;

            return result;
        }

        [Fact]
        public void RunStoredProcComplexTypeNoParameters()
        {
            using (NorthwindDbContext context = this.NorthwindContext())
            {
                List<MostExpensiveProductModel> mostExpensiveProducts = context.TenMostExpensiveProducts();
                Assert.True(mostExpensiveProducts.Count == 10, "Ten products were not found");
            }
        }

        [Fact]
        public void RunStoredProcComplexTypeWithParameters()
        {
            using (NorthwindDbContext context = this.NorthwindContext())
            {
                List<SalesByCategoryModel> salesByCategory = context.SalesByCategory("Beverages", 1998);

                using (MSSQLDbClient client = new MSSQLDbClient(ConnectionString))
                {
                    List<SalesByCategoryModel> salesByCategoryDirect = client.Fill<List<SalesByCategoryModel>>("exec [SalesByCategory] 'Beverages', 1998");
                    Assert.True(salesByCategory.Count == salesByCategoryDirect.Count, "Sales by category returned incorrect results.");
                }
            }
        }
    }
}
