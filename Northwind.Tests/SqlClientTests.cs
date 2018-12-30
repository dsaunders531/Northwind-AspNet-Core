﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using mezzanine.DbClient;
using Northwind.BLL.Models;

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
                List<ProductRowApiModel> productRowApiOs = client.Fill<List<ProductRowApiModel>>("SELECT * From Products");
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
                ProductRowApiModel productRowApiOs = client.Fill<ProductRowApiModel>("SELECT TOP 1 * FROM PRODUCTS");
                Assert.True(productRowApiOs.RowId == 1, "No products were found.");
            }
        }
    }
}
