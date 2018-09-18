using Microsoft.AspNetCore.Http;

namespace mezzanine.Extensions
{
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Get the request path
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public static string Path(this HttpContext me)
        {
            return me.Request?.Path;
        }

        /// <summary>
        /// Returns the text direction according to the user culture. Use the the <html> tag.
        /// </summary>
        /// <returns></returns>
        public static string TextDirection(this HttpContext me)
        {
            string strReturn = "ltr";
            if (me.Request?.Culture().TextInfo.IsRightToLeft == true)
            {
                strReturn = "rtl";
            }
            return strReturn;
        }

        /// <summary>
        /// Get a cookie from the request.
        /// </summary>
        /// <param name="me"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetCookie(this HttpContext me, string key)
        {
            string result = string.Empty;
            IRequestCookieCollection cookies = me.Request?.Cookies;

            if (cookies != null)
            {                
                cookies.TryGetValue(key, out result);                
            }

            return result;
        }

        /// <summary>
        /// Save a cookie in the response
        /// </summary>
        /// <param name="me"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="lifeTimeDays"></param>
        public static void SetCookie(this HttpContext me, string key, string value, int lifeTimeDays)
        {
            string result = string.Empty;
            IRequestCookieCollection cookies = me.Request?.Cookies;

            if (cookies.TryGetValue(key, out string temp) == true)
            {                
                me.DeleteCookie(key);
            }

            me.Response.CreateCookie(key, value, lifeTimeDays);
        }

        /// <summary>
        /// Delete a cookie from the response
        /// </summary>
        /// <param name="me"></param>
        /// <param name="key"></param>
        public static void DeleteCookie(this HttpContext me, string key)
        {
            me.Response.DeleteCookie(key);
        }
    }
}
