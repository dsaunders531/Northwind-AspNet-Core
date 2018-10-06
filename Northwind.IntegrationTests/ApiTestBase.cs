using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.IntegrationTests
{
    /// <summary>
    /// Base class for testing the Northwind Api
    /// </summary>
    public abstract class ApiTestBase
    {
        protected const string BaseUrl = "http://localhost:52869/api";
        protected const string ConnectionString = "Server=[server]\\[instance];Database=Northwind;Trusted_Connection=true;MultipleActiveResultSets=true";

        /// <summary>
        /// Get the headers with token
        /// </summary>
        protected List<KeyValuePair<string, string>> Headers
        {
            get
            {
                ApiAuthorizationTests loginTests = new ApiAuthorizationTests();
                loginTests.Login();
                string apiToken = loginTests.ApiToken;
                loginTests = null;

                return new List<KeyValuePair<string, string>>
                            {
                                new KeyValuePair<string, string>("ApiToken", apiToken)
                            };
            }
        }
    }
}
