// <copyright file="RESTClient.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using duncans.Serialization;
using duncans.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;

namespace duncans.RestClient
{
    /// <summary>
    /// A rest client. Provides higher level functions using RestSharp.
    /// </summary>
    public sealed class RESTClient : IDisposable
    {
        public RESTClient(string baseUrl, bool skipAvailabilityCheck, IAuthenticator authenticator, int timeout)
        {
            this.Startup(baseUrl, skipAvailabilityCheck, authenticator, timeout);
        }

        public RESTClient(string baseUrl, IAuthenticator authenticator, int timeout)
        {
            this.Startup(baseUrl, false, authenticator, timeout);
        }

        public RESTClient(string baseUrl, bool skipAvailabilityCheck, int timeout)
        {
            this.Startup(baseUrl, skipAvailabilityCheck, null, timeout);
        }

        public RESTClient(string baseUrl, int timeout)
        {
            this.Startup(baseUrl, false, null, timeout);
        }

        private RestSharp.RestClient RestClient { get; set; }

        public RestResult<TOutput> ExecuteXML<TOutput>(
                                        string resourceUrl,
                                        Method method,
                                        List<KeyValuePair<string, string>> queryParameters = null,
                                        List<KeyValuePair<string, string>> routeParameters = null,
                                        List<KeyValuePair<string, string>> headerParameters = null,
                                        List<KeyValuePair<string, string>> cookies = null,
                                        object xmlBody = null,
                                        string xmlBodyPartial = "")
        {
            RestResult<TOutput> result = new RestResult<TOutput>();

            try
            {
                RestResult initialResult = this.ExecuteXML(resourceUrl, method, queryParameters, routeParameters, headerParameters, cookies, xmlBody, xmlBodyPartial);

                using (Transposition transposition = new Transposition())
                {
                    result = transposition.Transpose(initialResult, result);

                    using (JSONSerializer serializer = new JSONSerializer())
                    {
                        // To make life easier creating the strongly typed models convert the XML to JSON then serialize it.
                        if (result.XmlContent != null)
                        {
                            string jsonContent = serializer.XmlToJSON(result.XmlContent);
                            jsonContent = jsonContent.Replace("\"@", "\"").Replace("\"#", "\"");
                            result.Result = serializer.Deserialize<TOutput>(jsonContent);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Exception = e;
                result.Message = e.Message;
            }

            return result;
        }

        public RestResult ExecuteXML(
                                        string resourceUrl,
                                        Method method,
                                        List<KeyValuePair<string, string>> queryParameters = null,
                                        List<KeyValuePair<string, string>> routeParameters = null,
                                        List<KeyValuePair<string, string>> headerParameters = null,
                                        List<KeyValuePair<string, string>> cookies = null,
                                        object xmlBody = null,
                                        string xmlBodyPartial = "")
        {
            RestResult result = new RestResult();

            try
            {
                RestRequest request = this.CreateRequest(resourceUrl, method, DataFormat.Xml, queryParameters, routeParameters, headerParameters, cookies, xmlBody, xmlBodyPartial);

                IRestResponse response = RestClient.Execute(request);

                result.StatusCode = (int)response.StatusCode;

                if (result.StatusCode == StatusCodes.Status500InternalServerError)
                {
                    result.Success = false;

                    result.Exception = new RestException("The request did not complete sucessfully.", response.ErrorException);

                    using (JSONSerializer serializer = new JSONSerializer())
                    {
                        // To make life easier creating the strongly typed models convert the XML to JSON then serialize it.
                        if (result.XmlContent != null)
                        {
                            string jsonContent = serializer.XmlToJSON(result.XmlContent);
                            jsonContent = jsonContent.Replace("\"@", "\"").Replace("\"#", "\"");
                            result.ServerError = serializer.Deserialize<InternalServerError>(jsonContent);
                        }
                    }
                }
                else if (result.StatusCode == StatusCodes.Status400BadRequest)
                {
                    result.Success = false;

                    using (JSONSerializer serializer = new JSONSerializer())
                    {
                        // To make life easier creating the strongly typed models convert the XML to JSON then serialize it.
                        if (result.XmlContent != null)
                        {
                            string jsonContent = serializer.XmlToJSON(result.XmlContent);
                            jsonContent = jsonContent.Replace("\"@", "\"").Replace("\"#", "\"");
                            result.ModelState = serializer.Deserialize<ModelStateDictionary>(jsonContent);
                        }
                    }
                }
                else
                {
                    result.Success = response.ErrorException == null;
                    result.Exception = response.ErrorException;
                    result.Content = response.Content;
                }

                result.Headers.AddRange(response.Headers);
                result.Cookies.AddRange(response.Cookies);

                if (response.Content != null)
                {
                    XmlDocument xd = new XmlDocument();
                    xd.LoadXml(response.Content);
                    result.XmlContent = xd;
                    xd = null;
                }
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Exception = e;
                result.Message = e.LongMessage();
            }

            return result;
        }

        /// <summary>
        /// Perform a rest request.
        /// </summary>
        /// <typeparam name="TOutput">The type of output object in RestResult.Result.</typeparam>
        /// <param name="resourceUrl">The route excluding the base url including any route parameters in curly braces.</param>
        /// <param name="method">The request method.</param>
        /// <param name="queryParameters">A list of query string parameters.</param>
        /// <param name="routeParameters">A list of route parameters, the keys must match the parameters in the resourceUrl.</param>
        /// <param name="headerParameters">Additional parameters to send in the header.</param>
        /// <param name="cookies">Additional parameters to send as cookies.</param>
        /// <param name="jsonBody">An object which will be put in the body in json format.</param>
        /// <param name="jsonBodyPartial">A string representing a partial json object.</param>
        /// <returns>A RestResult object.</returns>
        public RestResult<TOutput> Execute<TOutput>(
                            string resourceUrl,
                            Method method,
                            List<KeyValuePair<string, string>> queryParameters = null,
                            List<KeyValuePair<string, string>> routeParameters = null,
                            List<KeyValuePair<string, string>> headerParameters = null,
                            List<KeyValuePair<string, string>> cookies = null,
                            object jsonBody = null,
                            string jsonBodyPartial = "")
        {
            RestResult<TOutput> result = new RestResult<TOutput>();

            try
            {
                RestResult initialResult = this.Execute(resourceUrl, method, queryParameters, routeParameters, headerParameters, cookies, jsonBody, jsonBodyPartial);

                using (Transposition transposition = new Transposition())
                {
                    result = transposition.Transpose(initialResult, result);

                    if (initialResult.Content != null)
                    {
                        using (JSONSerializer serializer = new JSONSerializer())
                        {
                            result.Result = serializer.Deserialize<TOutput>(initialResult.Content);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Exception = e;
                result.Message = e.Message;
            }

            return result;
        }

        public RestResult Execute(
                            string resourceUrl,
                            Method method,
                            List<KeyValuePair<string, string>> queryParameters = null,
                            List<KeyValuePair<string, string>> routeParameters = null,
                            List<KeyValuePair<string, string>> headerParameters = null,
                            List<KeyValuePair<string, string>> cookies = null,
                            object jsonBody = null,
                            string jsonBodyPartial = "")
        {
            RestResult result = new RestResult();

            try
            {
                RestRequest request = this.CreateRequest(resourceUrl, method, DataFormat.Json, queryParameters, routeParameters, headerParameters, cookies, jsonBody, jsonBodyPartial);

                IRestResponse response = RestClient.Execute(request);

                result.StatusCode = (int)response.StatusCode;

                if (result.StatusCode == StatusCodes.Status500InternalServerError)
                {
                    result.Success = false;

                    result.Exception = new RestException("The request did not complete sucessfully.", response.ErrorException);

                    if (response.Content != null)
                    {
                        using (JSONSerializer serializer = new JSONSerializer())
                        {
                            try
                            {
                                result.ServerError = serializer.Deserialize<InternalServerError>(response.Content);
                            }
                            catch (Exception)
                            {
                                result.ServerError = new InternalServerError() { ExceptionType = response.ErrorException?.GetType().ToString(), Trace = string.Empty, Message = response.Content };
                            }
                        }
                    }
                }
                else if (result.StatusCode == StatusCodes.Status400BadRequest)
                {
                    result.Success = false;

                    using (JSONSerializer serializer = new JSONSerializer())
                    {
                        string modelStateString = response.Content;

                        // now remove the other fields \"things\":
                        string pattern = "\"\\w*\":"; // look for a text field
                        Regex found = new Regex(pattern);
                        MatchCollection mc = found.Matches(modelStateString);

                        result.ModelState = new ModelStateDictionary();

                        if (mc.Count > 0)
                        {
                            foreach (Match item in mc)
                            {
                                int valuePos = modelStateString.IndexOf(item.Value) + item.Value.Length;
                                int delimiterPos = modelStateString.Length;

                                try
                                {
                                    delimiterPos = modelStateString.IndexOf(",", valuePos);

                                    if (delimiterPos == -1)
                                    {
                                        // There is no comma in the output.
                                        delimiterPos = modelStateString.Length - 1;
                                    }
                                }
                                catch (Exception)
                                {
                                    delimiterPos = modelStateString.Length - 1;
                                }

                                string modelErr = modelStateString.Substring(valuePos + 2, (delimiterPos - valuePos) - 4);

                                result.ModelState.AddModelError(string.Empty, modelErr);
                            }
                        }
                        else if (modelStateString.Contains(":["))
                        {
                            int startPos = modelStateString.IndexOf(":[");

                            if (startPos != -1)
                            {
                                startPos = startPos + 2;

                                int endPos = modelStateString.IndexOf("]", startPos);

                                if (endPos != -1)
                                {
                                    string modelErr = modelStateString.Substring(startPos, endPos - startPos);

                                    result.ModelState.AddModelError(string.Empty, modelErr);
                                }
                            }
                        }
                    }
                }
                else
                {
                    result.Success = response.ErrorException == null;
                    result.Exception = response.ErrorException;
                    result.Content = response.Content;
                }

                result.Headers.AddRange(response.Headers);
                result.Cookies.AddRange(response.Cookies);
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Exception = e;
                result.Message = e.Message;
            }

            return result;
        }

        public void Dispose()
        {
            this.RestClient = null;
        }

        private RestRequest CreateRequest(
                            string resourceUrl,
                            Method method,
                            DataFormat format,
                            List<KeyValuePair<string, string>> queryParameters,
                            List<KeyValuePair<string, string>> routeParameters,
                            List<KeyValuePair<string, string>> headerParameters,
                            List<KeyValuePair<string, string>> cookies,
                            object body,
                            string bodyPartial)
        {
            RestRequest request = new RestRequest(resourceUrl, method) { RequestFormat = format };

            // add cookie values
            request = this.AddCookies(request, cookies);

            // add query parameters
            if ((method == Method.POST || method == Method.PUT) && queryParameters != null)
            {
                throw new ArgumentException("Query parameters cannot be used with POST or PUT methods.");
            }

            // add query parameters
            request = this.AddQueryParameters(request, queryParameters);

            // add route parameters
            request = this.AddRouteParameters(request, routeParameters);

            // add header parameters
            request = this.AddHeaderParameters(request, headerParameters);

            // add json body
            if (body != null)
            {
                switch (format)
                {
                    case DataFormat.Json:
                        request.AddJsonBody(body);
                        break;
                    case DataFormat.Xml:
                        request.AddXmlBody(body);
                        break;
                }
            }
            else if (bodyPartial != null)
            {
                if (bodyPartial.Length > 0)
                {
                    switch (format)
                    {
                        case DataFormat.Json:
                            request.Parameters.Add(new Parameter() { Name = "application/json", ContentType = null, Type = ParameterType.RequestBody, Value = bodyPartial });
                            break;
                        case DataFormat.Xml:
                            request.Parameters.Add(new Parameter() { Name = "application/xml", ContentType = null, Type = ParameterType.RequestBody, Value = bodyPartial });
                            break;
                    }
                }
            }

            return request;
        }

        private RestRequest AddCookies(RestRequest request, List<KeyValuePair<string, string>> cookies)
        {
            if (cookies != null)
            {
                if (cookies.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in cookies)
                    {
                        request.AddCookie(item.Key, item.Value.HTMLEncode());
                    }
                }
            }

            return request;
        }

        private RestRequest AddQueryParameters(RestRequest request, List<KeyValuePair<string, string>> queryParameters)
        {
            if (queryParameters != null)
            {
                if (queryParameters.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in queryParameters)
                    {
                        request.AddParameter(item.Key, item.Value.URLEncode());
                    }
                }
            }

            return request;
        }

        private RestRequest AddRouteParameters(RestRequest request, List<KeyValuePair<string, string>> routeParameters)
        {
            if (routeParameters != null)
            {
                if (routeParameters.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in routeParameters)
                    {
                        request.AddUrlSegment(item.Key, item.Value.URLEncode());
                    }
                }
            }

            return request;
        }

        private RestRequest AddHeaderParameters(RestRequest request, List<KeyValuePair<string, string>> headerParameters)
        {
            if (headerParameters != null)
            {
                if (headerParameters.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in headerParameters)
                    {
                        request.AddHeader(item.Key, item.Value.HTMLEncode());
                    }
                }
            }

            return request;
        }

        private bool ServiceAvailable()
        {
            bool result = false;

            try
            {
                HttpWebRequest rq = WebRequest.Create(this.RestClient.BaseUrl) as HttpWebRequest;
                rq.Method = "HEAD";
                HttpWebResponse rs = rq.GetResponse() as HttpWebResponse;

                result = rs.StatusCode == HttpStatusCode.OK;
                rs.Close();
                rs.Dispose();
                rs = null;
                rq = null;
            }
            catch
            {
                result = false;
            }

            return result;
        }

        private void Startup(string baseUrl, bool skipAvailabilityCheck, IAuthenticator authenticator, int timeout = 60000)
        {
            this.RestClient = new RestSharp.RestClient(baseUrl) { AutomaticDecompression = true, Timeout = timeout };
            this.RestClient.UseSerializer(() => new JSONSerializer());

            if (authenticator != null)
            {
                this.RestClient.Authenticator = authenticator;
            }

            if (skipAvailabilityCheck == false)
            {
                if (this.ServiceAvailable() == false)
                {
                    throw new WebException("Service at '" + this.RestClient.BaseUrl + "' unavailable.");
                }
            }
        }
    }

    internal class ModelStateResult
    {
        public string[] Values { get; set; }
    }
}
