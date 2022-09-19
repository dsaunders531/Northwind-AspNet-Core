using System;
using System.Reflection;

namespace tools.Utility
{
    /// <summary>
    /// Static class for converting objects into other objects
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
        /// <param name="input">An instance of the input object with values you want to set on the output object</param>
        /// <param name="output">The object which has some matching fields of the input object.</param>
        /// <returns></returns>
        public object Transpose(object input, object output)
        {
            object result = output;
            result = this.MapProperties(input.GetType(), input, output.GetType(), output);
            return result;
        }

        /// <summary>
        /// Take the public properties from the input object and set the value on matching properties on the output object.
        /// </summary>
        /// <typeparam name="OutputT">The type of output object</typeparam>
        /// <param name="input">An instance of the input object with values you want to set on the output object</param>
        /// <param name="output">The object which has some matching fields of the input object.</param>
        /// <returns></returns>
        public OutputT Transpose<OutputT>(object input, OutputT output)
        {
            OutputT result = output;
            result = (OutputT)this.MapProperties(input.GetType(), input, output.GetType(), (object)output);
            return result;
        }

        /// <summary>
        /// Take the public properties from the input object and set the value on matching properties on the output object.
        /// </summary>
        /// <typeparam name="InputT">The type of input object</typeparam>
        /// <typeparam name="OutputT">The type of output object</typeparam>
        /// <param name="input">An instance of the input object with values you want to set on the output object.</param>
        /// <param name="output">The object which has some matching fields of the input object.</param>
        /// <returns></returns>
        public OutputT Transpose<InputT, OutputT>(InputT input, OutputT output)
        {
            OutputT result = output;
            OutputT defaultOutput = Activator.CreateInstance<OutputT>();

            PropertyInfo[] inputProperties = input.GetType().GetProperties();

            for (int i = 0; i < inputProperties.Length; i++)
            {
                result = MapProperty<InputT, OutputT>(input, output, defaultOutput, inputProperties[i]);
            }

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
            // does the property exists in the output type?
            if (outputType?.GetProperty(propertyName) != null)
            {                
                object outputCurrentValue = outputType?.GetProperty(propertyName)?.GetValue(output);

                if (outputCurrentValue != null && propertyValue == null)
                {
                    // do nothing - partial objects must not overwrite database values
                }
                else
                {
                    outputType?.GetProperty(propertyName)?.SetValue(output, propertyValue);
                }
            }
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
            PropertyInfo[] inputProperties = inputType.GetProperties();

            for (int i = 0; i < inputProperties.Length; i++)
            {
                string propertyName = inputProperties[i].Name;
                object propertyValue = inputProperties[i].GetValue(input);
                Type propertyType = inputProperties[i].PropertyType;
                Type resultType = output.GetType().GetProperty(propertyName)?.PropertyType;

                if (resultType != null)
                {
                    if (resultType != propertyType && resultType.IsClass == true)
                    {
                        // The types are different and are classes.
                        object resultValue = this.MapProperties(propertyType, propertyValue, resultType, Activator.CreateInstance(resultType));
                        this.SetPropertyValue(output, outputType, propertyName, resultValue);
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
            }

            return output;
        }

        /// <summary>
        /// Mapping using generic types items.
        /// </summary>
        /// <typeparam name="InputT"></typeparam>
        /// <typeparam name="OutputT"></typeparam>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <param name="defaultOutput"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        private OutputT MapProperty<InputT, OutputT>(InputT input, OutputT output, OutputT defaultOutput, PropertyInfo property)
        {
            OutputT result = output;
            Type resultType = output.GetType();

            string propertyName = property.Name;
            object propertyValue = property.GetValue(input);
            Type propertyType = property.PropertyType;
            Type resultPropertyType = result.GetType().GetProperty(propertyName)?.PropertyType;

            if (resultPropertyType != null)
            {
                if (resultPropertyType != propertyType && resultPropertyType.IsClass == true)
                {
                    // The types are different and are classes.
                    if (propertyValue != null)
                    {
                        propertyValue = this.MapProperties(propertyType, propertyValue, resultPropertyType, Activator.CreateInstance(resultPropertyType));
                        this.SetPropertyValue(result, resultType, propertyName, propertyValue);
                    }
                }
                else
                {
                    if (resultPropertyType.IsValueType && propertyValue == null)
                    {
                        object defaultValue = property.GetValue(defaultOutput);
                        this.SetPropertyValue(result, resultType, propertyName, defaultValue);
                    }
                    else
                    {
                        this.SetPropertyValue(result, resultType, propertyName, propertyValue);
                    }
                }
            }

            return result;
        }
    }
}
