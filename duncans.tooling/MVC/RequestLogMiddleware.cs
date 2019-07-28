// <copyright file="RequestLogMiddleware.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using duncans.Utility;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace duncans.shared.Middleware
{
    /// <summary>
    /// TODO - not implemented yet.
    /// </summary>
    public class RequestLogMiddleware
    {
        public RequestLogMiddleware(RequestDelegate next)
        {
            this.Next = next;
        }

        private RequestDelegate Next { get; set; }

        public async Task Invoke(HttpContext context)
        {
            bool isAuth = context.User.Identity.IsAuthenticated;
            string user = context.User.Identity.Name.Encrypt(new BasicEncryption());
            IPAddress address = context.Connection.RemoteIpAddress;
            IHeaderDictionary headers = context.Request.Headers;
            string useragent = "unknown";

            if (headers.ContainsKey("User-Agent"))
            {
                useragent = headers["User-Agent"];
            }
            else
            {
                if (headers.ContainsKey("user-agent"))
                {
                    useragent = headers["user-agent"];
                }
            }

            string requestUrl = context.Request.PathAndQuery();
            DateTime arrived = DateTime.UtcNow;

            await this.Next.Invoke(context);
        }
    }
}
