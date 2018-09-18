using mezzanine.Extensions;
using Microsoft.AspNetCore.Mvc;
using mezzanine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace mezzanine.Utility
{
    /// <summary>
    /// Discover the api controllers and actions from an MVC assembly.
    /// You will need to fully decorate your controllers, actions and parameters to see meaningful output.
    /// </summary>
    public sealed class ApiDiscovery
    {
        Assembly Assembly { get; set; }

        public ApiDiscovery(Assembly assembly)
        {
            this.Assembly = assembly;
        }

        /// <summary>
        /// Discover all the api endpoints in an assembly.
        /// </summary>
        /// <returns></returns>
        public ApiControllersViewModel Discover()
        {
            ApiControllersViewModel result = new ApiControllersViewModel();
            System.Type[] assemblyTypes = this.Assembly.GetTypes();

            for (long i = 0; i < assemblyTypes.LongLength; i++)
            {
                System.Type assemblyType = assemblyTypes[i];

                // get the controller base types
                if (assemblyType.BaseType == typeof(Microsoft.AspNetCore.Mvc.Controller) || assemblyType.BaseType?.BaseType == typeof(Microsoft.AspNetCore.Mvc.Controller))
                {                                    
                    string controllerName = string.Empty;
                    string controllerRoute = string.Empty;
                    bool addController = false;
                    
                    addController = this.HasApiAttrs(assemblyType, out controllerName, out controllerRoute);

                    if (addController == true)
                    {
                        ApiControllerModel apiController = new ApiControllerModel() { Name = controllerName, Route = controllerRoute };
                        MethodInfo[] methods = assemblyType.GetMethods();
                        
                        // get the attributes
                        foreach (MethodInfo method in methods)
                        {
                            ApiActionModel apiActionModel = this.GetApiActionModel(method);

                            if (apiActionModel != null)
                            {
                                apiController.Actions.Add(apiActionModel);
                            }
                        }

                        result.Controllers.Add(apiController);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// See if the controller has api attributes
        /// </summary>
        /// <param name="assemblyType"></param>
        /// <param name="attrName"></param>
        /// <param name="route"></param>
        /// <returns></returns>
        private bool HasApiAttrs(System.Type assemblyType, out string attrName, out string route)
        {
            List<CustomAttributeData> attrs = assemblyType.CustomAttributes.ToList();
            bool result = false;
            attrName = string.Empty;
            route = string.Empty;

            foreach (CustomAttributeData attr in attrs)
            {
                if (attr.AttributeType == typeof(Microsoft.AspNetCore.Mvc.ApiControllerAttribute))
                {
                    // add controller to list and look at its action methods.
                    attrName = assemblyType.Name;
                    result = true;
                    break;
                }                
            }

            // loop through attributes and find the route
            if (result == true)
            {
                foreach (CustomAttributeData attr in attrs)
                {
                    if (attr.AttributeType == typeof(Microsoft.AspNetCore.Mvc.RouteAttribute))
                    {
                        // add controller to list and look at its action methods.                            
                        route = this.RouteAttributeValue(attr.ConstructorArguments);

                        if (route.Contains("[controller]"))
                        {
                            route = route.Replace("[controller]", attrName);
                        }

                        if (route.Contains("Controller"))
                        {
                            route = route.Replace("Controller", string.Empty);
                        }

                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Get the attributes and determine the method to work out how the action is to be called.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="apiMethod"></param>
        /// <param name="routeInfo"></param>
        /// <returns></returns>
        private bool FindApiMethod(MethodInfo method, out ApiMethod apiMethod, out string routeInfo)
        {
            bool result = false;
            apiMethod = ApiMethod.GET;
            routeInfo = string.Empty;
            Attribute methodAttr = null;

            methodAttr = method.GetCustomAttribute(typeof(HttpGetAttribute));

            if (methodAttr != null)
            {
                apiMethod = ApiMethod.GET;
                routeInfo = this.GetDefaultCustomAttributeValue(method, typeof(HttpGetAttribute));
                result = true;
            }
            else
            {
                methodAttr = method.GetCustomAttribute(typeof(HttpPostAttribute));

                if (methodAttr != null)
                {
                    apiMethod = ApiMethod.POST;
                    routeInfo = this.GetDefaultCustomAttributeValue(method, typeof(HttpPostAttribute));
                    result = true;
                }
                else
                {
                    methodAttr = method.GetCustomAttribute(typeof(HttpPutAttribute));

                    if (methodAttr != null)
                    {
                        apiMethod = ApiMethod.PUT;
                        routeInfo = this.GetDefaultCustomAttributeValue(method, typeof(HttpPutAttribute));
                        result = true;
                    }
                    else
                    {
                        methodAttr = method.GetCustomAttribute(typeof(HttpPatchAttribute));

                        if (methodAttr != null)
                        {
                            apiMethod = ApiMethod.PATCH;
                            routeInfo = this.GetDefaultCustomAttributeValue(method, typeof(HttpPutAttribute));
                            result = true;
                        }
                        else
                        {
                            methodAttr = method.GetCustomAttribute(typeof(HttpDeleteAttribute));

                            if (methodAttr != null)
                            {
                                apiMethod = ApiMethod.DELETE;
                                routeInfo = this.GetDefaultCustomAttributeValue(method, typeof(HttpDeleteAttribute));
                                result = true;
                            }
                        }
                    }
                }
            }           

            return result;
        }

        /// <summary>
        /// Get the custom attribute (decoration) value. The default or first value is returned.
        /// </summary>
        /// <param name="attributeDatas"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        private string GetDefaultCustomAttributeValue(MethodInfo method, Type targetType)
        {
            string result = string.Empty;
            IEnumerable<CustomAttributeData> attributeDatas = method.CustomAttributes;

            // Find additional route information
            foreach (CustomAttributeData attr in attributeDatas)
            {
                if (attr.AttributeType == targetType)
                {
                    result = this.RouteAttributeValue(attr.ConstructorArguments);
                    break;
                }
            }

            if (result.Contains("[action]"))
            {
                result = result.Replace("[action]", method.Name);
            }

            return result;
        }

        /// <summary>
        /// Get the action details for a controller method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        private ApiActionModel GetApiActionModel(MethodInfo method)
        {
            ApiActionModel result = new ApiActionModel() { Signature = method.ToString(), Name = method.Name };
            bool addAction = false;
            
            // determing the method
            ApiMethod apiMethod = ApiMethod.GET; // default
            string routeInfo = string.Empty;
            addAction = this.FindApiMethod(method, out apiMethod, out routeInfo);

            if (addAction == false)
            {
                result = null;
            }
            else
            {
                result.Method = apiMethod;
                result.Route = routeInfo;

                // See if there is a custom default response
                Attribute methodAttr = null;
                methodAttr = method.GetCustomAttribute(typeof(ProducesResponseTypeAttribute));

                if (methodAttr != null)
                {
                    result.SucessResponseCode = Convert.ToInt32(this.GetDefaultCustomAttributeValue(method, typeof(ProducesResponseTypeAttribute)));
                }

                // Get the input parameters                
                ParameterInfo[] parameters = method.GetParameters();

                foreach (ParameterInfo p in parameters)
                {
                    ApiActionParameterType parameterType = this.FindActionParameterType(p);                    

                    if (parameterType != ApiActionParameterType.indeterminate)
                    {
                        ApiActionParameterModel parameter = new ApiActionParameterModel()
                                                        {
                                                            Name = p.Name,
                                                            Type = p.ParameterType,
                                                            IsNullable =  p.IsOptional || p.ParameterType.ToString().Contains("Nullable"),
                                                            ParameterType = parameterType
                                                        };

                        if (parameterType == ApiActionParameterType.jsonBody)
                        {                           
                            parameter.DefaultValue = p.ParameterType.JSONExample().UnMinify();
                        }
                        else
                        {
                            parameter.DefaultValue = p.DefaultValue.ToString();
                        }

                        result.Parameters.Add(parameter);
                    }
                }

                // determine the output type.
                if (method.ReturnType.IsAbstract == false)
                {
                    if (method.ReturnType.IsGenericType)
                    {
                        // assuming the return type is always a single type.
                        result.ReturnType = method.ReturnType.GenericTypeArguments[0];
                    }
                    else
                    {
                        result.ReturnType = method.ReturnType;
                    }

                    result.ReturnBody = result.ReturnType.JSONExample().UnMinify();
                }
                
            }

            return result;
        }

        /// <summary>
        /// Find the parameter type (query, route, body) for the parameter
        /// </summary>
        /// <param name="parameterInfo"></param>
        /// <returns></returns>
        private ApiActionParameterType FindActionParameterType(ParameterInfo parameterInfo)
        {
            ApiActionParameterType result = ApiActionParameterType.indeterminate;

            foreach (CustomAttributeData paramAttr in parameterInfo.CustomAttributes.ToList<CustomAttributeData>())
            {
                if (paramAttr.AttributeType == typeof(FromBodyAttribute))
                {
                    result = ApiActionParameterType.jsonBody;
                    break;
                }

                if (paramAttr.AttributeType == typeof(FromQueryAttribute))
                {
                    result = ApiActionParameterType.query;
                    break;
                }

                if (paramAttr.AttributeType == typeof(FromRouteAttribute))
                {
                    result = ApiActionParameterType.route;
                    break;
                }

                if (paramAttr.AttributeType == typeof(FromHeaderAttribute))
                {
                    result = ApiActionParameterType.header;
                    break;
                }

                if (paramAttr.AttributeType == typeof(FromFormAttribute))
                {
                    result = ApiActionParameterType.form;
                    break;
                }
            }

            return result;
        }

        private string RouteAttributeValue(IList<CustomAttributeTypedArgument> namedArguments)
        {
            string result = string.Empty;

            if (namedArguments.Count > 0)
            {
                CustomAttributeTypedArgument item = namedArguments.First();
                result = item.Value.ToString();
            }
            
            return result;
        }
    }
}
