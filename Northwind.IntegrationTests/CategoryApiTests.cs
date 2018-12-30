using mezzanine.RestClient;
using mezzanine.DbClient;
using mezzanine.Utility;
using Northwind.BLL.Models;
using System.Collections.Generic;
using Xunit;

namespace Northwind.IntegrationTests
{
    /// <summary>
    /// Test the api endpoint for categories
    /// </summary>
    public class CategoryApiTests : ApiTestBase
    {
        private const string TestCategoryName = "Test Category";

        private string GetRecordSql
        {
            get
            {
                return "SELECT RowId, CategoryName, Description FROM Categories WHERE CategoryName = '" + TestCategoryName + "' ORDER BY RowId";
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
                RestResult<List<CategoryRowApiModel>> apiResult = restClient.Execute<List<CategoryRowApiModel>>("Category", RestSharp.Method.GET, headerParameters: Headers);
                Assert.True(apiResult.Success, apiResult.Content);
                Assert.NotNull(apiResult.Result);

                using (MSSQLDbClient sqlClient = new MSSQLDbClient(ConnectionString))
                {
                    List<CategoryRowApiModel> sqlResult = sqlClient.Fill<List<CategoryRowApiModel>>("SELECT RowId, CategoryName, Description FROM Categories ORDER BY RowId");
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

                RestResult<CategoryRowApiModel> apiResult = restClient.Execute<CategoryRowApiModel>("Category/{CategoryId}", RestSharp.Method.POST,routeParameters: routeParams, headerParameters: Headers);
                Assert.True(apiResult.Success, apiResult.Content);
                Assert.NotNull(apiResult);

                using (MSSQLDbClient sqlClient = new MSSQLDbClient(ConnectionString))
                {
                    CategoryRowApiModel sqlResult = sqlClient.Fill<CategoryRowApiModel>("SELECT RowId, CategoryName, Description FROM Categories WHERE RowId = 2 ORDER BY RowId");
                    Assert.NotNull(sqlResult);
                    Assert.True(apiResult.Result.RowId == sqlResult.RowId && apiResult.Result.CategoryName == sqlResult.CategoryName, "The records do not match.");
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

                RestResult<CategoryRowApiModel> apiResult = restClient.Execute<CategoryRowApiModel>("Category/{CategoryId}", RestSharp.Method.POST, routeParameters: routeParams, headerParameters: Headers);
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
                CategoryRowApiModel existingCategory = sqlClient.Fill<CategoryRowApiModel>(GetRecordSql);
                if (existingCategory != null)
                {
                    this.Delete();
                }

                using (RESTClient restClient = new RESTClient(BaseUrl))
                {
                    CategoryRowApiModel newCategory = new CategoryRowApiModel() { CategoryName = TestCategoryName, Description = "Some text about the item" };

                    RestResult<CategoryRowApiModel> apiResult = restClient.Execute<CategoryRowApiModel>("Category", RestSharp.Method.PUT, jsonBody: newCategory, headerParameters: Headers);
                    Assert.True(apiResult.StatusCode == 201, apiResult.Content);

                    CategoryRowApiModel sqlResult = sqlClient.Fill<CategoryRowApiModel>("SELECT RowId, CategoryName, Description FROM Categories WHERE CategoryName = '" + newCategory.CategoryName + "' ORDER BY RowId");
                    Assert.True(sqlResult.CategoryName == apiResult.Result.CategoryName 
                                && sqlResult.Description == apiResult.Result.Description 
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
                CategoryRowApiModel newCategory = new CategoryRowApiModel() { CategoryName = string.Empty, Description = "Some text about the item" };
                RestResult<CategoryRowApiModel> apiResult = restClient.Execute<CategoryRowApiModel>("Category", RestSharp.Method.PUT, jsonBody: newCategory, headerParameters: Headers);
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
                CategoryRowApiModel existingCategory = sqlClient.Fill<CategoryRowApiModel>(GetRecordSql);

                if (existingCategory == null)
                {
                    this.Create();
                    existingCategory = sqlClient.Fill<CategoryRowApiModel>(GetRecordSql);
                }

                Assert.NotNull(existingCategory);

                using (RESTClient restClient = new RESTClient(BaseUrl))
                {
                    CategoryRowApiModel updatedCategory = existingCategory;
                    updatedCategory.Description += " More text.";

                    RestResult<CategoryRowApiModel> apiResult = restClient.Execute<CategoryRowApiModel>("Category", RestSharp.Method.PATCH, jsonBody: updatedCategory, headerParameters: Headers);
                    Assert.True(apiResult.StatusCode == 200, apiResult.Content);
                    Assert.True(apiResult.Result.Description.EndsWith(" More text."), "The updated text was not returned");
                }

                CategoryRowApiModel dbCategory = sqlClient.Fill<CategoryRowApiModel>(GetRecordSql);
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

                RestResult<CategoryRowApiModel> apiResult = restClient.Execute<CategoryRowApiModel>("Category", RestSharp.Method.PATCH, jsonBodyPartial: partialJson, headerParameters: Headers);
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
                CategoryRowApiModel existingCategory = sqlClient.Fill<CategoryRowApiModel>(GetRecordSql);

                if (existingCategory == null)
                {
                    this.Create();
                    existingCategory = sqlClient.Fill<CategoryRowApiModel>(GetRecordSql);
                }

                Assert.NotNull(existingCategory);

                using (RESTClient restClient = new RESTClient(BaseUrl))
                {
                    string partialJson = "{ \"RowId\": " + existingCategory.RowId + ", \"Description\": \"All about more text\" }";
                    RestResult<CategoryRowApiModel> apiResult = restClient.Execute<CategoryRowApiModel>("Category", RestSharp.Method.PATCH, jsonBodyPartial: partialJson, headerParameters: Headers);
                    Assert.True(apiResult.StatusCode == 200, apiResult.Content);
                }

                CategoryRowApiModel dbCategory = sqlClient.Fill<CategoryRowApiModel>(GetRecordSql);
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
                CategoryRowApiModel existingCategory = sqlClient.Fill<CategoryRowApiModel>(GetRecordSql);

                if (existingCategory == null)
                {
                    this.Create();
                    existingCategory = sqlClient.Fill<CategoryRowApiModel>(GetRecordSql);
                }

                Assert.NotNull(existingCategory);

                using (RESTClient restClient = new RESTClient(BaseUrl))
                {
                    string partialJson = "{ \"RowId\": " + existingCategory.RowId + " }";
                    RestResult<CategoryRowApiModel> apiResult = restClient.Execute<CategoryRowApiModel>("Category", RestSharp.Method.PATCH, jsonBodyPartial: partialJson, headerParameters: Headers);
                    
                    // the assert should be false. The model we are working with is simple and has only 1 required parameter.
                    Assert.True(apiResult.StatusCode == 200, apiResult.Content);
                }

                CategoryRowApiModel dbCategory = sqlClient.Fill<CategoryRowApiModel>(GetRecordSql);
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
                CategoryRowApiModel existingCategory = sqlClient.Fill<CategoryRowApiModel>(GetRecordSql);

                if (existingCategory == null)
                {
                    this.Create();
                    existingCategory = sqlClient.Fill<CategoryRowApiModel>(GetRecordSql);
                }

                Assert.NotNull(existingCategory);

                using (RESTClient restClient = new RESTClient(BaseUrl))
                {
                    List<KeyValuePair<string, string>> routeParams = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("CategoryId", existingCategory.RowId.ToString())
                    };

                    RestResult<CategoryRowApiModel> apiResult = restClient.Execute<CategoryRowApiModel>("Category/{CategoryId}", RestSharp.Method.DELETE, routeParameters: routeParams, headerParameters: Headers);
                    Assert.True(apiResult.StatusCode == 301, apiResult.Content);
                }

                CategoryRowApiModel dbCategory = sqlClient.Fill<CategoryRowApiModel>(GetRecordSql);
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

                RestResult<CategoryRowApiModel> apiResult = restClient.Execute<CategoryRowApiModel>("Category/{CategoryId}", RestSharp.Method.DELETE, routeParameters: routeParams, headerParameters: Headers);
                Assert.True(apiResult.StatusCode == 500 || apiResult.StatusCode == 400 || apiResult.StatusCode == 204, apiResult.Content);
            }
        }
    }
}
