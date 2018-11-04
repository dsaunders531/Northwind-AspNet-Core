using mezzanine.RestClient;
using mezzanine.DbClient;
using mezzanine.Utility;
using Microsoft.AspNetCore.Http;
using Northwind.BLL.Models;
using Northwind.DAL.Models.Authentication;
using System.Collections.Generic;
using Xunit;
using RestSharp;

namespace Northwind.IntegrationTests
{
    public class ApiAuthorizationTests
    {
        private const string BaseUrl = "http://localhost:52869/api";

        private const string AuthCookieName = ".AspNetCore.Identity.Application";

        public string ApiToken { get; private set; }
        public RestResponseCookie ApiCookie { get; private set; }

        [Fact]
        public void Login()
        {
            ApiLoginModel model = new ApiLoginModel() { Email = "admin@example.com", Password = "Password1!" };

            using (RESTClient restClient = new RESTClient(BaseUrl))
            {
                RestResult apiResult = restClient.Execute("Authorize/Login", RestSharp.Method.POST, jsonBody: model);

                Assert.True(apiResult.StatusCode == StatusCodes.Status200OK, "Logging in was not sucessful.");

                this.ApiToken = apiResult.Headers.Find(h => h.Name == "ApiToken").Value.ToString();
                this.ApiCookie = apiResult.Cookies.Find(c => c.Name == AuthCookieName);

                Assert.True(this.ApiToken != string.Empty, "A token was not returned");
            }
        }

        [Fact]
        public void Logout()
        {
            if (this.ApiToken== null || this.ApiCookie == null)
            { 
                this.Login();
            }

            using (RESTClient restClient = new RESTClient(BaseUrl))
            {
                List<KeyValuePair<string, string>> headers = new List<KeyValuePair<string, string>>
                                                                {
                                                                    new KeyValuePair<string, string>("ApiToken", this.ApiToken)
                                                                };

               
                RestResult apiResult = restClient.Execute("Authorize/Logout", RestSharp.Method.GET, headerParameters: headers);

                this.ApiToken = string.Empty;

                Assert.True(apiResult.StatusCode == StatusCodes.Status200OK, "Logging out did not work.");
            }
        }
    }
}
