using RestSharp;
using System.Collections.Generic;

namespace tools.Models
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
        }

        public int StatusCode { get; set; } = -1;
        public string Content { get; set; } = string.Empty;
        public List<Parameter> Headers { get; set; }
        public List<RestResponseCookie> Cookies { get; set; }
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
