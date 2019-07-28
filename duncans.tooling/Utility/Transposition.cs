// <copyright file="Transposition.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System;
using System.Reflection;

namespace duncans.Utility
{
    /// <summary>
    /// Static class for converting objects into other objects.
    /// </summary>
    public class Transposition : IDisposable
    {
        public void Dispose()
        {
            // do nothing. this is needed so this class can be used in a using statement;
        }

        /// <summary>
        /// Take the public properties from the input object and set the value on matching properties on the output object.
        /// </summary>
        /// <typeparam name="TInput">The type of input object.</typeparam>
        /// <typeparam name="TOutput">The type of output object.</typeparam>
        /// <param name="input">An instance of the input object with values you want to set on the output object.</param>
        /// <param name="output">The object which has some matching fields of the input object.</param>
        /// <returns></returns
        public TOutput Transpose<TInput, TOutput>(TInput input, TOutput output)
        {
            TOutput result = output;
            result = (TOutput)this.MapProperties(typeof(TInput), input, typeof(TOutput), output);
            return result;
        }

        /// <summary>
        /// Set a on an object.
        /// </summary>
        /// <param name="output"></param>
        /// <param name="outputType"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        private void SetPropertyValue(object output, Type outputType, string propertyName, object propertyValue)
        {
            PropertyInfo propertyInfo = outputType.GetTopProperty(propertyName);

            // does the property exists in the output type?
            if (propertyInfo != null)
            {
                bool canWrite = propertyInfo.CanWrite;

                if (canWrite == true)
                {
                    object outputCurrentValue = propertyInfo.GetValue(output);

                    if (outputCurrentValue != null && propertyValue == null)
                    {
                        // do nothing - partial objects must not overwrite database values
                    }
                    else
                    {
                        propertyInfo.SetValue(output, propertyValue);
                    }
                }
            }

            propertyInfo = null;
        }

        /// <summary>
        /// Mapping using weakly typed items.
        /// </summary>
        /// <param name="inputType"></param>
        /// <param name="input"></param>
        /// <param name="outputType"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        private object MapProperties(Type inputType, object input, Type outputType, object output)
        {
            if (input != null)
            {
                PropertyInfo[] inputProperties = inputType.GetProperties();

                for (int i = 0; i < inputProperties.Length; i++)
                {
                    string propertyName = inputProperties[i].Name;

                    try
                    {
                        object propertyValue = inputProperties[i].GetValue(input);
                        Type propertyType = inputProperties[i].PropertyType;

                        // ambig on 'deleted' property
                        // could be output type is not clearly defined?
                        Type resultType = outputType.GetTopProperty(propertyName)?.PropertyType;

                        if (resultType != null)
                        {
                            bool isListType = resultType.IsSerializable == true && resultType.IsClass == true && resultType.IsGenericType && resultType.Name.Contains("List");

                            if (isListType == false)
                            {
                                if (resultType != propertyType && resultType.IsClass == true)
                                {
                                    // The types are different and are classes.
                                    try
                                    {
                                        object resultValue = this.MapProperties(propertyType, propertyValue, resultType, Activator.CreateInstance(resultType));
                                        this.SetPropertyValue(output, outputType, propertyName, resultValue);
                                    }
                                    catch (MissingMethodException)
                                    {
                                        // Do nothing. Objects which do not have parameterless constuctors cannot be mappped.
                                    }
                                }
                                else
                                {
                                    if (resultType.IsValueType && propertyValue == null)
                                    {
                                        object defaultValue = Activator.CreateInstance(resultType);
                                        this.SetPropertyValue(output, outputType, propertyName, defaultValue);
                                    }
                                    else
                                    {
                                        this.SetPropertyValue(output, outputType, propertyName, propertyValue);
                                    }
                                }
                            }
                            else
                            {
                                //throw new ApplicationException("Transposition is not capable of remapping lists or arrays. You will need to create an inner loop to map these items.");
                            }
                        }
                    }
                    catch (TargetInvocationException e)
                    {
                        if (e.InnerException.GetType() == typeof(NotImplementedException))
                        {
                            // An exception can happen if a get has a not implemented exception.
                            // Ignore these.
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }

            return output;
        }
    }
}
