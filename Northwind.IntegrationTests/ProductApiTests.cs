using mezzanine.RestClient;
using mezzanine.DbClient;
using mezzanine.Utility;
using Northwind.BLL.Models;
using System.Collections.Generic;
using Xunit;

namespace Northwind.IntegrationTests
{
    public class ProductApiTests : ApiTestBase
    {        
        private const string TestProductName = "Test Product";

        private string GetRecordSql
        {
            get
            {
                return "SELECT RowId, ProductName, SupplierID as SupplierId, CategoryID as CategoryId, QuantityPerUnit, "
                        + "UnitPrice, UnitsInStock, UnitsOnOrder, ReorderLevel, Discontinued FROM Products WHERE ProductName = '" + TestProductName + "' ORDER BY RowId";
            }
        }

        /// <summary>
        /// See if getting all the records works.
        /// </summary>
        [Fact]
        public void GetAll()
        {
            using (RESTClient restClient = new RESTClient(BaseUrl))
            {
                RestResult<List<ProductRowApiModel>> apiResult = restClient.Execute<List<ProductRowApiModel>>("Product", RestSharp.Method.GET, headerParameters: Headers);
                Assert.True(apiResult.Success, apiResult.Content);
                Assert.NotNull(apiResult.Result);

                using (MSSQLDbClient sqlClient = new MSSQLDbClient(ConnectionString))
                {
                    List<ProductRowApiModel> sqlResult = sqlClient.Fill<List<ProductRowApiModel>>("SELECT RowId, ProductName, SupplierID as SupplierId, CategoryID as CategoryId, QuantityPerUnit, "
                                                                                            + "UnitPrice, UnitsInStock, UnitsOnOrder, ReorderLevel, Discontinued FROM Products ORDER BY RowId");
                    Assert.NotNull(sqlResult);

                    Assert.True(apiResult.Result.Count == sqlResult.Count, "The record counts do not match.");
                }
            }
        }

        /// <summary>
        /// Get one record from the database.
        /// </summary>
        [Fact]
        public void GetOne()
        {
            using (RESTClient restClient = new RESTClient(BaseUrl))
            {
                List<KeyValuePair<string, string>> routeParams = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("key", "2")
                };

                RestResult<ProductRowApiModel> apiResult = restClient.Execute<ProductRowApiModel>("Product/{key}", RestSharp.Method.POST, routeParameters: routeParams, headerParameters: Headers);
                Assert.True(apiResult.Success, apiResult.Content);
                Assert.NotNull(apiResult);

                using (MSSQLDbClient sqlClient = new MSSQLDbClient(ConnectionString))
                {
                    ProductRowApiModel sqlResult = sqlClient.Fill<ProductRowApiModel>("SELECT RowId, ProductName, SupplierID as SupplierId, CategoryID as CategoryId, QuantityPerUnit, "
                                                                                            + "UnitPrice, UnitsInStock, UnitsOnOrder, ReorderLevel, Discontinued FROM Products WHERE RowId = 2 ORDER BY RowId");
                    Assert.NotNull(sqlResult);
                    Assert.True(apiResult.Result.RowId == sqlResult.RowId && apiResult.Result.ProductName == sqlResult.ProductName, "The records do not match.");
                }
            }
        }

        /// <summary>
        /// Try to get a non-existant record from the api.
        /// </summary>
        [Fact]
        public void GetNonExistant()
        {
            using (RESTClient restClient = new RESTClient(BaseUrl))
            {
                List<KeyValuePair<string, string>> routeParams = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("key", "1000")
                };

                RestResult<ProductRowApiModel> apiResult = restClient.Execute<ProductRowApiModel>("Product/{key}", RestSharp.Method.POST, routeParameters: routeParams, headerParameters: Headers);
                Assert.True(apiResult.StatusCode == 204, apiResult.Content);
                Assert.Null(apiResult.Result);
            }
        }

        /// <summary>
        /// Create a record.
        /// </summary>
        [Fact]
        public void Create()
        {
            using (MSSQLDbClient sqlClient = new MSSQLDbClient(ConnectionString))
            {
                ProductRowApiModel existing = sqlClient.Fill<ProductRowApiModel>(GetRecordSql);
                if (existing != null)
                {
                    this.Delete();
                }

                using (RESTClient restClient = new RESTClient(BaseUrl))
                {
                    ProductRowApiModel newItem = new ProductRowApiModel() { ProductName = TestProductName, CategoryId = 2, QuantityPerUnit = "4 in a box", UnitPrice = (decimal)12.37, SupplierId = 1 };

                    RestResult<ProductRowApiModel> apiResult = restClient.Execute<ProductRowApiModel>("Product", RestSharp.Method.PUT, jsonBody: newItem, headerParameters: Headers);
                    Assert.True(apiResult.StatusCode == 201, apiResult.Content);

                    ProductRowApiModel sqlResult = sqlClient.Fill<ProductRowApiModel>(GetRecordSql);
                    Assert.True(sqlResult.ProductName == apiResult.Result.ProductName 
                                    && sqlResult.CategoryId == apiResult.Result.CategoryId 
                                    && sqlResult.QuantityPerUnit == apiResult.Result.QuantityPerUnit
                                    && sqlResult.RowId == apiResult.Result.RowId);
                }
            }
        }

        /// <summary>
        /// Create a record using incorrect data.
        /// </summary>
        [Fact]
        public void CreateInvalid()
        {
            using (RESTClient restClient = new RESTClient(BaseUrl))
            {
                ProductRowApiModel newItem = new ProductRowApiModel() { ProductName = string.Empty, UnitPrice = 0, SupplierId = -3, CategoryId = -4 };
                RestResult<CategoryRowApiModel> apiResult = restClient.Execute<CategoryRowApiModel>("Product", RestSharp.Method.PUT, jsonBody: newItem, headerParameters: Headers);
                Assert.True(apiResult.StatusCode == 406 || apiResult.StatusCode == 400, apiResult.Content);
            }
        }

        /// <summary>
        /// Update an existing record
        /// </summary>
        [Fact]
        public void Update()
        {
            using (MSSQLDbClient sqlClient = new MSSQLDbClient(ConnectionString))
            {
                ProductRowApiModel existing = sqlClient.Fill<ProductRowApiModel>(GetRecordSql);

                if (existing == null)
                {
                    this.Create();
                    existing = sqlClient.Fill<ProductRowApiModel>(GetRecordSql);
                }

                Assert.NotNull(existing);

                using (RESTClient restClient = new RESTClient(BaseUrl))
                {
                    ProductRowApiModel updated = existing;
                    updated.UnitPrice = 123;

                    RestResult<ProductRowApiModel> apiResult = restClient.Execute<ProductRowApiModel>("Product", RestSharp.Method.PATCH, jsonBody: updated, headerParameters: Headers);
                    Assert.True(apiResult.StatusCode == 200, apiResult.Content);
                    Assert.True(apiResult.Result.UnitPrice == 123, "The updated value was not returned");
                }

                ProductRowApiModel dbValue = sqlClient.Fill<ProductRowApiModel>(GetRecordSql);
                Assert.True(dbValue.UnitPrice == 123, "The updated value was not returned");
            }
        }

        /// <summary>
        /// Update a record with incorrect data
        /// </summary>
        [Fact]
        public void UpdateInvalid()
        {
            using (RESTClient restClient = new RESTClient(BaseUrl))
            {
                string partialJson = "{ \"RowId\": -12345, \"CategoryId\": 2, \"SupplierId\":1, \"UnitPrice\": 0 }";

                RestResult<ProductRowApiModel> apiResult = restClient.Execute<ProductRowApiModel>("Product", RestSharp.Method.PATCH, jsonBodyPartial: partialJson, headerParameters: Headers);
                Assert.True(apiResult.StatusCode == 406 || apiResult.StatusCode == 400 || apiResult.StatusCode == 204, apiResult.Content);
            }
        }

        /// <summary>
        /// Update part of a record.
        /// </summary>
        [Fact]
        public void UpdatePartial()
        {
            using (MSSQLDbClient sqlClient = new MSSQLDbClient(ConnectionString))
            {
                ProductRowApiModel existing = sqlClient.Fill<ProductRowApiModel>(GetRecordSql);

                if (existing == null)
                {
                    this.Create();
                    existing = sqlClient.Fill<ProductRowApiModel>(GetRecordSql);
                }

                Assert.NotNull(existing);

                using (RESTClient restClient = new RESTClient(BaseUrl))
                {
                    string partialJson = "{ \"RowId\": " + existing.RowId + ", \"CategoryId\": 2, \"SupplierId\": 1, \"UnitPrice\": 32.1 }";
                    RestResult<ProductRowApiModel> apiResult = restClient.Execute<ProductRowApiModel>("Product", RestSharp.Method.PATCH, jsonBodyPartial: partialJson, headerParameters: Headers);
                    Assert.True(apiResult.StatusCode == 200, apiResult.Content);

                    ProductRowApiModel dbValue = sqlClient.Fill<ProductRowApiModel>(GetRecordSql);
                    Assert.True(dbValue.UnitPrice == (decimal)32.1, "Incorrect text was saved.");
                }
            }
        }

        /// <summary>
        /// Update part of a record badly.
        /// </summary>
        [Fact]
        public void UpdatePartialBadly()
        {
            using (MSSQLDbClient sqlClient = new MSSQLDbClient(ConnectionString))
            {
                ProductRowApiModel existingCategory = sqlClient.Fill<ProductRowApiModel>(GetRecordSql);

                if (existingCategory == null)
                {
                    this.Create();
                    existingCategory = sqlClient.Fill<ProductRowApiModel>(GetRecordSql);
                }

                Assert.NotNull(existingCategory);

                using (RESTClient restClient = new RESTClient(BaseUrl))
                {
                    string partialJson = "{ \"RowId\": " + existingCategory.RowId + " }";
                    RestResult<ProductRowApiModel> apiResult = restClient.Execute<ProductRowApiModel>("Product", RestSharp.Method.PATCH, jsonBodyPartial: partialJson, headerParameters: Headers);
                    
                    Assert.False(apiResult.StatusCode == 200, apiResult.Content);
                }

                ProductRowApiModel dbValue = sqlClient.Fill<ProductRowApiModel>(GetRecordSql);
                Assert.False(dbValue.ProductName == string.Empty, "Incorrect text was saved.");
            }
        }

        /// <summary>
        /// Delete a record
        /// </summary>
        [Fact]
        public void Delete()
        {
            using (MSSQLDbClient sqlClient = new MSSQLDbClient(ConnectionString))
            {
                ProductRowApiModel existingCategory = sqlClient.Fill<ProductRowApiModel>(GetRecordSql);

                if (existingCategory == null)
                {
                    this.Create();
                    existingCategory = sqlClient.Fill<ProductRowApiModel>(GetRecordSql);
                }

                Assert.NotNull(existingCategory);

                using (RESTClient restClient = new RESTClient(BaseUrl))
                {
                    List<KeyValuePair<string, string>> routeParams = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("key", existingCategory.RowId.ToString())
                    };

                    RestResult<ProductRowApiModel> apiResult = restClient.Execute<ProductRowApiModel>("Product/{key}", RestSharp.Method.DELETE, routeParameters: routeParams, headerParameters: Headers);
                    Assert.True(apiResult.StatusCode == 301, apiResult.Content);
                }

                ProductRowApiModel dbValue = sqlClient.Fill<ProductRowApiModel>(GetRecordSql);
                Assert.Null(dbValue);
            }
        }

        /// <summary>
        /// Delete a record with invalid data
        /// </summary>
        [Fact]
        public void DeleteInvalid()
        {
            using (RESTClient restClient = new RESTClient(BaseUrl))
            {
                List<KeyValuePair<string, string>> routeParams = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("key", "-1234")
                };

                RestResult<CategoryRowApiModel> apiResult = restClient.Execute<CategoryRowApiModel>("Product/{key}", RestSharp.Method.DELETE, routeParameters: routeParams, headerParameters: Headers);
                Assert.True(apiResult.StatusCode == 500 || apiResult.StatusCode == 400 || apiResult.StatusCode == 204, apiResult.Content);
            }
        }
    }
}
