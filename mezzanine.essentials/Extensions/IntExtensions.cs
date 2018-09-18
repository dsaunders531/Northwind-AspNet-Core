using Microsoft.AspNetCore.Http;

namespace mezzanine.Extensions
{
    public static class IntExtensions
    {
        /// <summary>
        /// Return text (en-GB) for the status code. The request is used to provide extra information in the response.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static string ToStatusCodeMeaning(this int statusCode, HttpRequest origin = default(HttpRequest))
        {
            string result = string.Empty;

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Status
            switch (statusCode)
            {
                // Information codes
                case 100:
                    result = string.Format("Code {0}. Continue.", statusCode);
                    break;
                case 101:
                    result = string.Format("Code {0}. Switching Protocol (Upgrade in request header).", statusCode);
                    break;
                case 102:
                    result = string.Format("Code {0}. Processing.", statusCode);
                    break;

                // Sucess
                case 200:
                    result = string.Format("Code {0}. OK.", statusCode);
                    break;
                case 201:
                    result = string.Format("Code {0}. Created.", statusCode);
                    break;
                case 203:
                    result = string.Format("Code {0}. Non-authoritative information (meta information does not match origin server).", statusCode);
                    break;
                case 204:
                    result = string.Format("Code {0}. No content.", statusCode);
                    break;
                case 205:
                    result = string.Format("Code {0}. Reset content.", statusCode);
                    break;
                case 206:
                    result = string.Format("Code {0}. Partial Content.", statusCode);
                    break;
                case 207:
                    result = string.Format("Code {0}. Multistatus (WebDAV).", statusCode);
                    break;
                case 208:
                    result = string.Format("Code {0}. Multistatus (WebDAV - Property status).", statusCode);
                    break;
                case 226:
                    result = string.Format("Code {0}. Delta encoding - partial update of existing client entity. IM Used.", statusCode);
                    break;

                // Redirections
                case 300:
                    result = string.Format("Code {0}. Multi choice. There is more than one redirection possible.", statusCode);
                    break;
                case 301:
                    result = string.Format("Code {0}. Page moved permanently.", statusCode);
                    break;
                case 302:
                    result = string.Format("Code {0}. URI {1} Found.", statusCode, origin.Path);
                    break;
                case 303:
                    result = string.Format("Code {0}. See other (there is another URI in the GET request).", statusCode);
                    break;
                case 304:
                    result = string.Format("Code {0}. Not modified.", statusCode);
                    break;
                case 305:
                    result = string.Format("Code {0}. A proxy must be used (be suspicious).", statusCode);
                    break;
                case 306:
                    result = string.Format("Code {0}. Unused (http 1.1 reserved code).", statusCode);
                    break;
                case 307:
                    result = string.Format("Code {0}. Temporary Redirect.", statusCode);
                    break;
                case 308:
                    result = string.Format("Code {0}. Permanent Redirect.", statusCode);
                    break;

                // Client error responses
                case 400:
                    result = string.Format("Code {0}. Bad request. The server does not understand what it recieved. (Check the URL, form or cookie data).", statusCode);
                    break;
                case 401:
                    result = string.Format("Code {0}. Unauthorized. You are not allowed to access this resource.", statusCode);
                    break;
                case 402:
                    result = string.Format("Code {0}. Payment required.", statusCode);
                    break;
                case 403:
                    result = string.Format("Code {0}. Forbidden. We know who you are. You are not allowed to access this resource.", statusCode);
                    break;
                case 404:
                    result = string.Format("Code {0}. Page not found.", statusCode, origin.Path);
                    break;
                case 405:
                    result = string.Format("Code {0}. Method {1} Not Allowed.", statusCode, origin.Method);
                    break;
                case 406:
                    result = string.Format("Code {0}. Not Acceptable. The server is unable to find any matching content.", statusCode);
                    break;
                case 407:
                    result = string.Format("Code {0}. Proxy Authentication Required. You are not allowed to access this resource unless authorised via a proxy.", statusCode);
                    break;
                case 408:
                    result = string.Format("Code {0}. Request Timeout.", statusCode);
                    break;
                case 409:
                    result = string.Format("Code {0}. Conflict. The request conflicts with the server state.", statusCode);
                    break;
                case 410:
                    result = string.Format("Code {0}. Gone. The resource {1} has been moved or permanently deleted with no forwarding address.", statusCode, origin.Path);
                    break;
                case 411:
                    result = string.Format("Code {0}. Length Required. The content-length header is missing.", statusCode);
                    break;
                case 412:
                    result = string.Format("Code {0}. Precondition Failed. There are missing headers in the request.", statusCode);
                    break;
                case 413:
                    result = string.Format("Code {0}. Payload Too Large. Request is too big for the server to handle.", statusCode);
                    break;
                case 414:
                    result = string.Format("Code {0}. URI {1} Too Long.", statusCode, origin.Path);
                    break;
                case 415:
                    result = string.Format("Code {0}. Unsupported Media Type.", statusCode);
                    break;
                case 416:
                    result = string.Format("Code {0}. Requested Range Not Satisfiable. The range header is incorrect (the data may be smaller than the range specified).", statusCode);
                    break;
                case 417:
                    result = string.Format("Code {0}. Expectation Failed. The requests expect field cannot be met by the server.", statusCode);
                    break;
                case 418:
                    result = string.Format("Code {0}. Insufficient drainage in the lower field.", statusCode);
                    break;
                case 421:
                    result = string.Format("Code {0}. Misdirected Request.", statusCode);
                    break;
                case 422:
                    result = string.Format("Code {0}. Unprocessable Entity (WebDAV).", statusCode);
                    break;
                case 423:
                    result = string.Format("Code {0}. Resource is locked. (WebDAV).", statusCode);
                    break;
                case 424:
                    result = string.Format("Code {0}. Failed Dependency. (WebDAV)", statusCode);
                    break;
                case 426:
                    result = string.Format("Code {0}. Upgrade Required. A different protocol is required (check the upgrade header in the response).", statusCode);
                    break;
                case 428:
                    result = string.Format("Code {0}. Precondition Required. The request needs to be conditional.", statusCode);
                    break;
                case 429:
                    result = string.Format("Code {0}. Too Many Requests. Your request limit has been reached. Go and do something else.", statusCode);
                    break;
                case 431:
                    result = string.Format("Code {0}. Request Header Fields Too Large.", statusCode);
                    break;
                case 452:
                    result = string.Format("Code {0}. {1} Unavailable For Legal Reasons. The content you are trying to access has been censored.", statusCode, origin.Path);
                    break;

                // Server responses
                case 500:
                    result = string.Format("Code {0}. Internal Server Error.", statusCode);
                    break;
                case 501:
                    result = string.Format("Code {0}. Method {1} Not Implemented.", statusCode, origin.Method);
                    break;
                case 502:
                    result = string.Format("Code {0}. Bad Gateway.", statusCode);
                    break;
                case 503:
                    result = string.Format("Code {0}. Service Unavailable. The server is not ready to process your request.", statusCode);
                    break;
                case 504:
                    result = string.Format("Code {0}. Gateway Timeout.", statusCode);
                    break;
                case 505:
                    result = string.Format("Code {0}. HTTP Version Not Supported.", statusCode);
                    break;
                case 506:
                    result = string.Format("Code {0}. Circular reference detected. Variant Also Negotiates.", statusCode);
                    break;
                case 507:
                    result = string.Format("Code {0}. Internal configuration error. Content negotiation does not result in an end point. Insufficient Storage.", statusCode);
                    break;
                case 508:
                    result = string.Format("Code {0}. Loop Detected (WebDAV). An infinite loop was detected while processing the request. Fire the developer.", statusCode);
                    break;
                case 510:
                    result = string.Format("Code {0}. Not Extended. Request extensions are missing.", statusCode);
                    break;
                case 511:
                    result = string.Format("Code {0}. Access Denied. Network Authentication Required.", statusCode);
                    break;

                default:
                    result = string.Format("Unknown status code {0}.", statusCode);
                    break;
            }

            return result;
        }
    }
}
