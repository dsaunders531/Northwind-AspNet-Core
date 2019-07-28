// <copyright file="ObjectExtensions.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using duncans.Serialization;
using duncans.Utility;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace duncans
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object me)
        {
            using (JSONSerializer serializer = new JSONSerializer())
            {
                return serializer.Serialize(me);
            }
        }

        /// <summary>
        /// Get the value of a property in an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static T GetPropertyValue<T>(this object me, string propertyName)
        {
            T result = default(T);

            foreach (PropertyInfo item in me.GetType().GetProperties())
            {
                if (item.Name == propertyName)
                {
                    result = (T)item.GetValue(me);
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Get the value of a property in an object.
        /// </summary>
        /// <param name="me"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetPropertyValue(this object me, string propertyName)
        {
            object result = null;

            foreach (PropertyInfo item in me.GetType().GetProperties())
            {
                if (item.Name == propertyName)
                {
                    result = item.GetValue(me);
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Return all the attribute metadata associated with a property.
        /// </summary>
        /// <param name="me"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        /// <remarks>Works best on EF annotated properties.</remarks>
        public static IList<CustomAttributeData> GetPropertyMetadata(this object me, string propertyName)
        {
            IList<CustomAttributeData> result = null;

            foreach (PropertyInfo item in me.GetType().GetProperties())
            {
                if (item.Name == propertyName)
                {
                    result = item.GetCustomAttributesData();

                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Get the [Display] property attribute value
        /// </summary>
        /// <param name="me"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        /// <remarks>Works best on EF annotated properties.</remarks>
        public static string GetDisplayName(this object me, string propertyName)
        {
            string result = propertyName; // default same as property name.

            IList<CustomAttributeData> attrs = me.GetPropertyMetadata(propertyName);

            if (attrs != null)
            {
                foreach (CustomAttributeData item in attrs)
                {
                    if (item.AttributeType == typeof(System.ComponentModel.DataAnnotations.DisplayAttribute))
                    {
                        foreach (CustomAttributeNamedArgument arg in item.NamedArguments)
                        {
                            if (arg.MemberName == "Name")
                            {
                                result = arg.TypedValue.ToString();
                                break;
                            }
                        }

                        break;
                    }
                }
            }

            return result;
        }

        public static Type GetPropertyType(this object me, string propertyName)
        {
            Type result = null;

            foreach (PropertyInfo item in me.GetType().GetProperties())
            {
                if (item.Name == propertyName)
                {
                    result = item.PropertyType;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Get the next value for a primitive object type. Only works for numbers, dates and guids.
        /// </summary>
        /// <param name="me"></param>
        /// <param name="T"></param>
        /// <returns></returns>
        public static object Increment(this object me, Type T)
        {
            if (T.IsPrimitive)
            {
                switch (T.Name)
                {
                    case "Byte":
                        me = (Byte)me + 1;
                        break;
                    case "SByte":
                        me = (SByte)me + 1;
                        break;
                    case "Int16":
                        me = (Int16)me + 1;
                        break;
                    case "UInt16":
                        me = (UInt16)me + 1;
                        break;
                    case "Int32":
                        me = (Int32)me + 1;
                        break;
                    case "UInt32":
                        me = (UInt32)me + 1;
                        break;
                    case "Int64":
                        me = (Int64)me + 1;
                        break;
                    case "UInt64":
                        me = (UInt64)me + 1;
                        break;
                    case "Double":
                        me = (Double)me + 1;
                        break;
                    case "Single":
                        me = (Single)me + 1;
                        break;
                    default:
                        throw new ArgumentException("Increment can only be used on Byte, SByte, Int16, UInt16, Int32, UInt32, Int64, UInt64, Double, Single, DateTime and Guid");
                }
            }
            else
            {
                switch (T.Name)
                {
                    case "DateTime":
                        me = ((DateTime)me).AddMinutes(1);
                        break;
                    case "Guid":
                        me = Guid.NewGuid();
                        break;
                    default:
                        throw new ArgumentException("Increment can only be used on Byte, SByte, Int16, UInt16, Int32, UInt32, Int64, UInt64, Double, Single, DateTime and Guid");
                }
            }

            return me;
        }

        /// <summary>
        /// Create a new copy of object in a seperate memory area (a new variable).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="me"></param>
        /// <returns></returns>
        public static T Clone<T>(this T me)
        {
            T result = Activator.CreateInstance<T>();

            using (Transposition transposition = new Transposition())
            {
                result = transposition.Transpose<T, T>(me, result);
            }

            return result;
        }
    }
}
