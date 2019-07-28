// <copyright file="XMLSerializer.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using RestSharp;
using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace duncans.Serialization
{
    /// <summary>
    /// Perform serialization operations using XML data.
    /// </summary>
    public class XMLSerializer : Serializer
    {
        public override string[] SupportedContentTypes { get; } =
        {
                "application/xml", "text/xml", "*+xml"
        };

        public override DataFormat DataFormat { get; } = DataFormat.Xml;

        public override string ContentType { get; set; } = "application/xml";

        public T Deserialize<T>(XmlDocument xmlDocument)
        {
            return this.Deserialize<T>(xmlDocument, new XmlRootAttribute(base.ObjectNameForXml(typeof(T))));
        }

        public T Deserialize<T>(XmlDocument xmlDocument, string rootNodeName)
        {
            return this.Deserialize<T>(xmlDocument, new XmlRootAttribute(rootNodeName));
        }

        public T Deserialize<T>(XmlDocument xmlDocument, XmlRootAttribute rootAttr)
        {
            T result = default(T);

            using (MemoryStream ms = new MemoryStream())
            {
                xmlDocument.Save(ms);
                ms.Flush(); // write all
                ms.Seek(0, SeekOrigin.Begin); // move to start

                XmlSerializer xs = new XmlSerializer(typeof(T), rootAttr);

                result = (T)xs.Deserialize(ms);

                xs = null;
            }

            return result;
        }

        public T Deserialize<T>(string XMLFilePath)
        {
            XmlDocument x = new XmlDocument();
            x.Load(XMLFilePath);
            return this.Deserialize<T>(x);
        }

        /// <summary>
        /// Converts any object to XML and returns the XML element. No need to manually type the convertion of an object to XML!
        /// </summary>
        /// <returns></returns>
        /// <param name="type"></param>
        /// <param name="o"></param>
        /// <param name="rootAttr"></param>
        public XmlElement Serialize(Type type, object o, XmlRootAttribute rootAttr)
        {
            MemoryStream ms = new MemoryStream();
            XmlSerializer xs = new XmlSerializer(type, rootAttr);
            XmlDocument outDoc = new XmlDocument();
            XmlElement returnElement = null;

            xs.Serialize(ms, o);
            ms.Flush(); // write all
            ms.Seek(0, SeekOrigin.Begin); // move to start

            outDoc.Load(ms);

            // Create the element with extra attributes.
            returnElement = outDoc.CreateElement(outDoc.DocumentElement.Name);

            // Add attibutes to say this is a converted thing
            XmlAttribute xa = outDoc.CreateAttribute("xmlns:xsi");
            xa.Value = "http://www.w3.org/2001/XMLSchema-instance";
            returnElement.Attributes.Append(xa);

            xa = outDoc.CreateAttribute("xmlns:xsd");
            xa.Value = "http://www.w3.org/2001/XMLSchema";
            returnElement.Attributes.Append(xa);

            returnElement.InnerXml = outDoc.DocumentElement.InnerXml;

            ms.Dispose();
            ms = null;
            xs = null;
            outDoc = null;

            return returnElement;
        }

        public XmlElement Serialize(Type type, object o, string rootNodeName)
        {
            return Serialize(type, o, new XmlRootAttribute(rootNodeName));
        }

        public XmlElement Serialize(Type type, object o)
        {
            return Serialize(type, o, new XmlRootAttribute(base.ObjectNameForXml(type)));
        }

        /// <summary>
        /// Convert document attributes to elements - required for clean deserialisation.
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <returns></returns>
        public XmlDocument DocumentAttrsToElements(XmlDocument xmlDocument)
        {
            this.AttributesToElements(xmlDocument.DocumentElement);

            foreach (XmlElement item in xmlDocument.DocumentElement.ChildNodes)
            {
                this.AttributesToElements(item);
            }

            return xmlDocument;
        }

        /// <summary>
        /// The serialiser ignores attributes so these need to be converted to elements.
        /// </summary>
        /// <param name="xmlElement"></param>
        /// <returns></returns>
        public XmlElement AttributesToElements(XmlElement xmlElement)
        {
            if (xmlElement.HasAttributes == true)
            {
                foreach (XmlAttribute attr in xmlElement.Attributes)
                {
                    XmlElement newEle = xmlElement.OwnerDocument.CreateElement(attr.Name);
                    newEle.InnerText = attr.Value;
                    xmlElement.AppendChild(newEle);
                }

                xmlElement.Attributes.RemoveAll();
            }

            return xmlElement;
        }

        public override T Deserialize<T>(IRestResponse response)
        {
            XmlDocument x = new XmlDocument();
            x.LoadXml(response.Content);
            return this.Deserialize<T>(x);
        }

        public override string Serialize(Parameter parameter)
        {
            if (this.SupportedContentTypes.Contains(parameter.ContentType))
            {
                return this.Serialize(parameter.Value);
            }
            else
            {
                throw new NotSupportedException(string.Format("The content type '0' is not supported by the serializer '{1}'", parameter.ContentType, this.GetType().ToString()));
            }
        }

        public override string Serialize(object obj)
        {
            return this.Serialize(obj);
        }
    }
}
