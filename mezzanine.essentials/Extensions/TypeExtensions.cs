using mezzanine.Utility;
using System;
using System.Linq;

namespace mezzanine.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Create a JSON example for this type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string JSONExample(this Type type)
        {
            string result = string.Empty;

            using (JSONSerialiser js = new JSONSerialiser())
            {
                try
                {
                    // see if a default can be created
                    if (type.IsGenericType == true)
                    {
                        // handle List<T> output
                        if (type.GenericTypeArguments.Length > 0)
                        {
                            result = js.Serialize(Activator.CreateInstance(type.GenericTypeArguments[0], true));
                            result = "[" + result + "]";
                        }
                    }
                    else
                    {
                        result = js.Serialize(Activator.CreateInstance(type, true));
                    }
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                }
            }

            return result;
        }

        /// <summary>
        /// Returns a short string (without the namespace) for the type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string ToShortString(this Type type)
        {
            string result = type.ToString().Replace(type.Namespace, string.Empty).Replace("`1", string.Empty);

            if (result.Contains("["))
            {
                // remove any parts from the array or list.
                int start = result.IndexOf("[") + 1;
                string objFullName = result.Substring(start, result.IndexOf("]") - start);

                if (objFullName.Contains("."))
                {
                    string objName = objFullName.Split(".").Last();
                    result = result.Replace(objFullName, objName);
                }
            }

            return result.Replace(".", string.Empty);
        }
    }
}
