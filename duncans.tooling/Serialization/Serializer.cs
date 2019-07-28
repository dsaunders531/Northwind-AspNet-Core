// <copyright file="Serializer.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serialization;
using System;
using System.Linq;
using System.Xml;

namespace duncans.Serialization
{
    /// <summary>
    /// Base serializer class.
    /// </summary>
    public abstract class Serializer : IDisposable, IRestSerializer
    {
        public abstract string[] SupportedContentTypes { get;  }

        public abstract DataFormat DataFormat { get; }

        public abstract string ContentType { get; set; }

        public abstract T Deserialize<T>(IRestResponse response);

        public void Dispose()
        {
            // There are no large objects to dispose but I want to use the serialisers in a using statement ...
        }

        /// <summary>
        /// You will need the object name for object convertion.
        /// </summary>
        /// <param name="typeToConvert"></param>
        /// <returns></returns>
        public string ObjectNameForXml(Type typeToConvert)
        {
            string result = typeToConvert.ToString();

            if (result.Contains("[") == true && result.Contains("]") == true)
            {
                // get the underlying object name
                int startPos = result.IndexOf("[") + 1;
                int len = result.IndexOf("]") - startPos;

                if (len > 0)
                {
                    result = result.Substring(startPos, len);
                }
                else
                {
                    result = result.Replace("[", string.Empty);
                    result = result.Replace("]", string.Empty);
                }

                if (result.Last().ToString() == "y")
                {
                    result = result.Replace("y", "ies");
                }
                else
                {
                    result += "s";
                }
            }

            if (result.Contains(".") == true)
            {
                result = result.Split(Convert.ToChar(".")).Last();
            }

            return result;
        }

        public abstract string Serialize(Parameter parameter);

        public abstract string Serialize(object obj);

        public string XmlToJSON(XmlElement xml)
        {
            return JsonConvert.SerializeXmlNode(xml);
        }

        public string XmlToJSON(XmlDocument xmlDoc)
        {
            return XmlToJSON(xmlDoc.DocumentElement);
        }
    }
}
