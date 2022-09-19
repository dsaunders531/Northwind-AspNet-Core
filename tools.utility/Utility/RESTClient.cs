//using tools.Models;
//using RestSharp;
//using System;
//using System.Collections.Generic;

//namespace tools.Utility
//{
//    /// <summary>
//    /// A rest client. Provides higher level functions using RestSharp
//    /// </summary>
//    public sealed class RESTClient : IDisposable
//    {
//        private RestClient RestClient { get; set; }

//        public RESTClient(string baseUrl)
//        {
//            this.RestClient = new RestClient(baseUrl);                   
//        }

//        /// <summary>
//        /// Perform a rest request.
//        /// </summary>
//        /// <typeparam name="OutputT">The type of output object in RestResult.Result</typeparam>
//        /// <param name="resourceUrl">The route excluding the base url including any route parameters in curly braces.</param>
//        /// <param name="method">The request method.</param>
//        /// <param name="queryParameters">A list of query string parameters.</param>
//        /// <param name="routeParameters">A list of route parameters, the keys must match the parameters in the resourceUrl.</param>
//        /// <param name="headerParameters">Additional parameters to send in the header.</param>
//        /// <param name="jsonBody">An object which will be put in the body in json format.</param>
//        /// <param name="jsonBodyPartial">A string representing a partial json object.</param>
//        /// <returns>A RestResult object</returns>
//        public RestResult<OutputT> Execute<OutputT>(string resourceUrl, 
//                            Method method, 
//                            List<KeyValuePair<string, string>> queryParameters = null, 
//                            List<KeyValuePair<string, string>> routeParameters = null,
//                            List<KeyValuePair<string, string>> headerParameters = null,
//                            List<KeyValuePair<string, string>> cookies = null,
//                            object jsonBody = null,
//                            string jsonBodyPartial = "")
//        {
//            RestResult<OutputT> result = new RestResult<OutputT>();

//            try
//            {
//                RestResult initialResult = this.Execute(resourceUrl, method, queryParameters, routeParameters, headerParameters, cookies, jsonBody, jsonBodyPartial);

//                using (Transposition transposition = new Transposition())
//                {
//                    result = transposition.Transpose(initialResult, result);
//                    using (JSONSerialiser serializer = new JSONSerialiser())
//                    {
//                        result.Result = serializer.Deserialize<OutputT>(initialResult.Content);
//                    }
//                }                
//            }
//            catch (Exception e) 
//            {
//                result.Success = false;
//                result.Exception = e;
//                result.Message = e.Message;
//            }
            
//            return result;
//        }

//        public RestResult Execute(string resourceUrl,
//                            Method method,
//                            List<KeyValuePair<string, string>> queryParameters = null,
//                            List<KeyValuePair<string, string>> routeParameters = null,
//                            List<KeyValuePair<string, string>> headerParameters = null,
//                            List<KeyValuePair<string, string>> cookies = null,
//                            object jsonBody = null,
//                            string jsonBodyPartial = "")
//        {
//            RestResult result = new RestResult();
            
//            try
//            {
//                RestRequest request = this.CreateRequest(resourceUrl, method, queryParameters, routeParameters, headerParameters, cookies, jsonBody, jsonBodyPartial);

//                IRestResponse response = RestClient.Execute(request);

//                result.StatusCode = (int)response.StatusCode;
//                result.Content = response.Content;
//                result.Headers.AddRange(response.Headers);
//                result.Cookies.AddRange(response.Cookies);
//            }
//            catch (Exception e)
//            {
//                result.Success = false;
//                result.Exception = e;
//                result.Message = e.Message;
//            }

//            return result;
//        }

//        private RestRequest CreateRequest(string resourceUrl,
//                            Method method,
//                            List<KeyValuePair<string, string>> queryParameters,
//                            List<KeyValuePair<string, string>> routeParameters,
//                            List<KeyValuePair<string, string>> headerParameters,
//                            List<KeyValuePair<string, string>> cookies,
//                            object jsonBody, 
//                            string jsonBodyPartial)
//        {
//            RestRequest request = new RestRequest(resourceUrl, method) { RequestFormat = DataFormat.Json };

//            // add cookie values
//            request = this.AddCookies(request, cookies);

//            // add query parameters
//            request = this.AddQueryParameters(request, queryParameters);

//            // add route parameters
//            request = this.AddRouteParameters(request, routeParameters);

//            // add header parameters
//            request = this.AddHeaderParameters(request, headerParameters);

//            // add json body
//            if (jsonBody != null)
//            {
//                request.AddJsonBody(jsonBody);
//            }
//            else if (jsonBodyPartial != null)
//            {
//                if (jsonBodyPartial.Length > 0)
//                {
//                    request.Parameters.Add(new Parameter() { Name = "application/json", ContentType = null, Type = ParameterType.RequestBody, Value = jsonBodyPartial });
//                }
//            }

//            return request;
//        }

//        private RestRequest AddCookies(RestRequest request, List<KeyValuePair<string, string>> cookies)
//        {
//            if (cookies != null)
//            {
//                if (cookies.Count > 0)
//                {
//                    foreach (KeyValuePair<string, string> item in cookies)
//                    {
//                        request.AddCookie(item.Key, item.Value);
//                    }
//                }
//            }

//            return request;
//        }

//        private RestRequest AddQueryParameters(RestRequest request, List<KeyValuePair<string, string>> queryParameters)
//        {
//            if (queryParameters != null)
//            {
//                if (queryParameters.Count > 0)
//                {
//                    foreach (KeyValuePair<string, string> item in queryParameters)
//                    {
//                        request.AddParameter(item.Key, item.Value);
//                    }
//                }
//            }

//            return request;
//        }

//        private RestRequest AddRouteParameters(RestRequest request, List<KeyValuePair<string, string>> routeParameters)
//        {
//            if (routeParameters != null)
//            {
//                if (routeParameters.Count > 0)
//                {
//                    foreach (KeyValuePair<string, string> item in routeParameters)
//                    {
//                        request.AddUrlSegment(item.Key, item.Value);
//                    }
//                }
//            }

//            return request;
//        }

//        private RestRequest AddHeaderParameters(RestRequest request, List<KeyValuePair<string, string>> headerParameters)
//        {
//            if (headerParameters != null)
//            {
//                if (headerParameters.Count > 0)
//                {
//                    foreach (KeyValuePair<string, string> item in headerParameters)
//                    {
//                        request.AddHeader(item.Key, item.Value);
//                    }
//                }
//            }

//            return request;
//        }

//        public void Dispose()
//        {
//            this.RestClient = null;
//        }
//    }
//}
