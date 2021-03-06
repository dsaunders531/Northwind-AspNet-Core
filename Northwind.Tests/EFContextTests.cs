﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using mezzanine.Utility;
using Northwind.BLL.Models;
using Northwind.DAL;
using Microsoft.EntityFrameworkCore;
using Northwind.DAL.Models;

namespace Northwind.Tests
{
    public class EFContextTests
    {
        private const string ConnectionString = "Server=[server]\\[instance];Database=Northwind;Trusted_Connection=true;MultipleActiveResultSets=true";

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
            using (NorthwindContext context = this.NorthwindContext())
            {
                List<MostExpensiveProduct> mostExpensiveProducts = context.TenMostExpensiveProducts();
                Assert.True(mostExpensiveProducts.Count == 10, "Ten products were not found");
            }
        }

        [Fact]
        public void RunStoredProcComplexTypeWithParameters()
        {
            using (NorthwindContext context = this.NorthwindContext())
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
