using mezzanine.Utility;
using Microsoft.AspNetCore.Http;
using System;

namespace mezzanine.Extensions
{
    /// <summary>
    /// Extensions for Session.
    /// </summary>
    public static class SessionExtensions
    {        
        /// <summary>
        /// Save an object in the session in Json Format.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <remarks>Use GetJSon to retrieve the object.></remarks>
        public static void SetJSon(this ISession session, string key, object value)
        {
            using (JSONSerialiser js = new JSONSerialiser())
            {
                string sessionData = js.Serialize(value).ToBase64();
                session.SetString(key, sessionData);
            }
        }

        /// <summary>
        /// Retrieve a value from the session which has been saved in JSON format.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <remarks>Save the value using SetJson.</remarks>
        public static T GetJSon<T>(this ISession session, string key)
        {
            string sessionData = null;
            T result = default(T);

            using (JSONSerialiser js = new JSONSerialiser())
            {
                sessionData = session.GetString(key).FromBase64();
                result = sessionData == null ? default(T) : js.Deserialize<T>(sessionData);
            }

            return result;
        }

        /// <summary>
        /// Overload of the built-in ISession.SetXML which saves the data base64 encoded.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <param name="T"></param>
        /// <param name="value"></param>
        public static void SetXML(this ISession session, string key, Type T, object value)
        {
            using (XMLSerializer serializer = new XMLSerializer())
            {
                string sessionData = serializer.Serialize(T, value).ToString();
                session.Set(key, sessionData.ToBytes());
            }
        }

        /// <summary>
        /// Overload of the built-in ISession.GetXML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetXML<T>(this ISession session, string key)
        {
            string sessionData = null;
            T result = default(T);

            using (XMLSerializer xs = new XMLSerializer())
            {
                sessionData = session.Get(key).FromBytes();
                result = sessionData == null ? default(T) : xs.Deserialize<T>(sessionData);
            }

            return result;
        }
    }
}
