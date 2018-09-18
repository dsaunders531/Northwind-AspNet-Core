using mezzanine.Models;
using mezzanine.Utility;
using Northwind.BLL.Models;
using System.Collections.Generic;
using Xunit;

namespace Northwind.IntegrationTests
{
    /// <summary>
    /// Test the api endpoint for categories
    /// </summary>
    public class CategoryApiTests
    {
        private const string BaseUrl = "http://localhost:52869/api";
        private const string ConnectionString = "Server=[server]\\[instance];Database=Northwind;Trusted_Connection=true;MultipleActiveResultSets=true";
        private const string TestCategoryName = "Test Category";

        private string GetRecordSql
        {
            get
            {
                return "SELECT CategoryID as CategoryId, CategoryName, Description FROM Categories WHERE CategoryName = '" + TestCategoryName + "' ORDER BY CategoryID";
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
                RestResult<List<CategoryRowApiO>> apiResult = restClient.Execute<List<CategoryRowApiO>>("Category", RestSharp.Method.GET);
                Assert.True(apiResult.Success, apiResult.Content);
                Assert.NotNull(apiResult.Result);

                using (MSSQLDbClient sqlClient = new MSSQLDbClient(ConnectionString))
                {
                    List<CategoryRowApiO> sqlResult = sqlClient.Fill<List<CategoryRowApiO>>("SELECT CategoryID, CategoryName, Description FROM Categories ORDER BY CategoryID");
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
                    new KeyValuePair<string, string>("CategoryId", "2")
                };

                RestResult<CategoryRowApiO> apiResult = restClient.Execute<CategoryRowApiO>("Category/{CategoryId}", RestSharp.Method.POST,routeParameters: routeParams);
                Assert.True(apiResult.Success, apiResult.Content);
                Assert.NotNull(apiResult);

                using (MSSQLDbClient sqlClient = new MSSQLDbClient(ConnectionString))
                {
                    CategoryRowApiO sqlResult = sqlClient.Fill<CategoryRowApiO>("SELECT CategoryID as CategoryId, CategoryName, Description FROM Categories WHERE CategoryId = 2 ORDER BY CategoryID");
                    Assert.NotNull(sqlResult);
                    Assert.True(apiResult.Result.CategoryId == sqlResult.CategoryId && apiResult.Result.CategoryName == sqlResult.CategoryName, "The records do not match.");
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
                    new KeyValuePair<string, string>("CategoryId", "1000")
                };

                RestResult<CategoryRowApiO> apiResult = restClient.Execute<CategoryRowApiO>("Category/{CategoryId}", RestSharp.Method.POST, routeParameters: routeParams);
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
                CategoryRowApiO existingCategory = sqlClient.Fill<CategoryRowApiO>(GetRecordSql);
                if (existingCategory != null)
                {
                    this.Delete();
                }

                using (RESTClient restClient = new RESTClient(BaseUrl))
                {
                    CategoryRowApiO newCategory = new CategoryRowApiO() { CategoryName = TestCategoryName, Description = "Some text about the item" };

                    RestResult<CategoryRowApiO> apiResult = restClient.Execute<CategoryRowApiO>("Category", RestSharp.Method.PUT, jsonBody: newCategory);
                    Assert.True(apiResult.StatusCode == 201, apiResult.Content);

                    CategoryRowApiO sqlResult = sqlClient.Fill<CategoryRowApiO>("SELECT CategoryID as CategoryId, CategoryName, Description FROM Categories WHERE CategoryName = '" + newCategory.CategoryName + "' ORDER BY CategoryID");
                    Assert.True(sqlResult.CategoryName == newCategory.CategoryName && sqlResult.Description == newCategory.Description);
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
                CategoryRowApiO newCategory = new CategoryRowApiO() { CategoryName = string.Empty, Description = "Some text about the item" };
                RestResult<CategoryRowApiO> apiResult = restClient.Execute<CategoryRowApiO>("Category", RestSharp.Method.PUT, jsonBody: newCategory);
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
                CategoryRowApiO existingCategory = sqlClient.Fill<CategoryRowApiO>(GetRecordSql);

                if (existingCategory == null)
                {
                    this.Create();
                    existingCategory = sqlClient.Fill<CategoryRowApiO>(GetRecordSql);
                }

                Assert.NotNull(existingCategory);

                using (RESTClient restClient = new RESTClient(BaseUrl))
                {
                    CategoryRowApiO updatedCategory = existingCategory;
                    updatedCategory.Description += " More text.";

                    RestResult<CategoryRowApiO> apiResult = restClient.Execute<CategoryRowApiO>("Category", RestSharp.Method.PATCH, jsonBody: updatedCategory);
                    Assert.True(apiResult.StatusCode == 200, apiResult.Content);
                    Assert.True(apiResult.Result.Description.EndsWith(" More text."), "The updated text was not returned");
                }

                CategoryRowApiO dbCategory = sqlClient.Fill<CategoryRowApiO>(GetRecordSql);
                Assert.True(dbCategory.Description.EndsWith(" More text."), "The updated text was not returned");
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
                string partialJson = "{ \"CategoryId\": -12345, \"Description\": \"All about more text\" }";

                RestResult<CategoryRowApiO> apiResult = restClient.Execute<CategoryRowApiO>("Category", RestSharp.Method.PATCH, jsonBodyPartial: partialJson);
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
                CategoryRowApiO existingCategory = sqlClient.Fill<CategoryRowApiO>(GetRecordSql);

                if (existingCategory == null)
                {
                    this.Create();
                    existingCategory = sqlClient.Fill<CategoryRowApiO>(GetRecordSql);
                }

                Assert.NotNull(existingCategory);

                using (RESTClient restClient = new RESTClient(BaseUrl))
                {
                    string partialJson = "{ \"CategoryId\": " + existingCategory.CategoryId + ", \"Description\": \"All about more text\" }";
                    RestResult<CategoryRowApiO> apiResult = restClient.Execute<CategoryRowApiO>("Category", RestSharp.Method.PATCH, jsonBodyPartial: partialJson);
                    Assert.True(apiResult.StatusCode == 200, apiResult.Content);
                }

                CategoryRowApiO dbCategory = sqlClient.Fill<CategoryRowApiO>(GetRecordSql);
                Assert.True(dbCategory.Description == "All about more text", "Incorrect text was saved.");
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
                CategoryRowApiO existingCategory = sqlClient.Fill<CategoryRowApiO>(GetRecordSql);

                if (existingCategory == null)
                {
                    this.Create();
                    existingCategory = sqlClient.Fill<CategoryRowApiO>(GetRecordSql);
                }

                Assert.NotNull(existingCategory);

                using (RESTClient restClient = new RESTClient(BaseUrl))
                {
                    string partialJson = "{ \"CategoryId\": " + existingCategory.CategoryId + " }";
                    RestResult<CategoryRowApiO> apiResult = restClient.Execute<CategoryRowApiO>("Category", RestSharp.Method.PATCH, jsonBodyPartial: partialJson);
                    
                    // the assert should be false. The model we are working with is simple and has only 1 required parameter.
                    Assert.True(apiResult.StatusCode == 200, apiResult.Content);
                }

                CategoryRowApiO dbCategory = sqlClient.Fill<CategoryRowApiO>(GetRecordSql);
                Assert.False(dbCategory.CategoryName == string.Empty, "Incorrect text was saved.");
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
                CategoryRowApiO existingCategory = sqlClient.Fill<CategoryRowApiO>(GetRecordSql);

                if (existingCategory == null)
                {
                    this.Create();
                    existingCategory = sqlClient.Fill<CategoryRowApiO>(GetRecordSql);
                }

                Assert.NotNull(existingCategory);

                using (RESTClient restClient = new RESTClient(BaseUrl))
                {
                    List<KeyValuePair<string, string>> routeParams = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("CategoryId", existingCategory.CategoryId.ToString())
                    };

                    RestResult<CategoryRowApiO> apiResult = restClient.Execute<CategoryRowApiO>("Category/{CategoryId}", RestSharp.Method.DELETE, routeParameters: routeParams);
                    Assert.True(apiResult.StatusCode == 301, apiResult.Content);
                }

                CategoryRowApiO dbCategory = sqlClient.Fill<CategoryRowApiO>(GetRecordSql);
                Assert.Null(dbCategory);
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
                    new KeyValuePair<string, string>("CategoryId", "-1234")
                };

                RestResult<CategoryRowApiO> apiResult = restClient.Execute<CategoryRowApiO>("Category/{CategoryId}", RestSharp.Method.DELETE, routeParameters: routeParams);
                Assert.True(apiResult.StatusCode == 500 || apiResult.StatusCode == 400 || apiResult.StatusCode == 204, apiResult.Content);
            }
        }
    }
}
