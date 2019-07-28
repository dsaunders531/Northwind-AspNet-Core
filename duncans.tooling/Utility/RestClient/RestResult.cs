// <copyright file="RestResult.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using duncans.WorkerPattern;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RestSharp;
using System.Collections.Generic;
using System.Xml;

namespace duncans.RestClient
{
    /// <summary>
    /// An object to store the results of a rest request.
    /// </summary>
    public class RestResult : WorkerResult
    {
        public RestResult()
        {
            this.Headers = new List<Parameter>();
            this.Cookies = new List<RestResponseCookie>();
            this.XmlContent = null;
            this.Content = null;
            this.ModelState = null;
            this.ServerError = null;
        }

        public int StatusCode { get; set; } = StatusCodes.Status418ImATeapot;

        public string Content { get; set; } = string.Empty;

        public XmlDocument XmlContent { get; set; } = null;

        public List<Parameter> Headers { get; set; }

        public List<RestResponseCookie> Cookies { get; set; }

        /// <summary>
        /// Gets or sets the model state when the status code is BadRequest.
        /// </summary>
        public ModelStateDictionary ModelState { get; set; }

        /// <summary>
        /// Gets or sets the server error status when the status code is InternalServerError.
        /// </summary>
        public InternalServerError ServerError { get; set; }

        /// <summary>
        /// Gets any error message
        /// </summary>
        public string ErrorMessage
        {
            get
            {
                string result = this.Message;

                if (this.ServerError != null)
                {
                    result += "\r\n" + this.ServerError?.Message + "\r\n" + this.ServerError?.Trace;
                }

                if (this.Exception != null)
                {
                    result += "\r\n" + this.Exception.Message;
                }

                return result.Trim();
            }
        }
    }

    /// <summary>
    /// An object to store the results of a rest request.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RestResult<T> : RestResult
    {
        public T Result { get; set; } = default(T);
    }
}
