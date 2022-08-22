using mezzanine.Utility;
using Northwind.BLL.Models;
using System.Collections.Generic;
using Xunit;

namespace Northwind.Tests
{
    public class SqlClientTests
    {
        private const string ConnectionString = "Server=LAPTOP10\\SQLEXPRESS;Database=Northwind;Trusted_Connection=true;MultipleActiveResultSets=true";

        /// <summary>
        /// See that the SqlClient Fill method works with many records.
        /// </summary>
        [Fact]
        public void TestFillMany()
        {
            using (MSSQLDbClient client = new MSSQLDbClient(ConnectionString))
            {
                List<ProductRowApiO> productRowApiOs = client.Fill<List<ProductRowApiO>>("SELECT * From Products");
                Assert.True(productRowApiOs.Count > 0, "No products were found.");
            }
        }

        /// <summary>
        /// See that the SqlClient fill method works with one record
        /// </summary>
        [Fact]
        public void TestFillOne()
        {
            using (MSSQLDbClient client = new MSSQLDbClient(ConnectionString))
            {
                ProductRowApiO productRowApiOs = client.Fill<ProductRowApiO>("SELECT TOP 1 * FROM PRODUCTS");
                Assert.True(productRowApiOs.ProductId == 1, "No products were found.");
            }
        }
    }
}
