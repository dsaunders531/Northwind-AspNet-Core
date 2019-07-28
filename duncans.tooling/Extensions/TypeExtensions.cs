// <copyright file="TypeExtensions.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using duncans.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace duncans
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

            using (JSONSerializer js = new JSONSerializer())
            {
                try
                {
                    // see if a default can be created
                    if (type.IsGenericType == true)
                    {
                        if (type.Name.StartsWith("List"))
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
                            result = js.Serialize(Activator.CreateInstance(type));
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
                    string objName = objFullName.Split(Convert.ToChar(".")).Last();
                    result = result.Replace(objFullName, objName);
                }
            }

            return result.Replace(".", string.Empty);
        }

        /// <summary>
        /// Get the property info from a types public property.
        /// This function avoids the AmbiguousMatchException by getting the top level (ie: overridden) property.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static PropertyInfo GetTopProperty(this Type type, string propertyName)
        {
            PropertyInfo result = null;

            try
            {
                result = type.GetProperty(propertyName);
            }
            catch (AmbiguousMatchException)
            {
                result = type.GetProperties()
                                                .ToList()
                                                .Where(p => p.Name == propertyName && p.DeclaringType == type)
                                                .FirstOrDefault();
            }

            return result;
        }
    }
}
