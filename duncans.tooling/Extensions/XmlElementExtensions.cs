// <copyright file="XmlElementExtensions.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System.Xml;

namespace duncans.tooling.Extensions
{
    public static class XmlElementExtensions
    {
        /// <summary>
        /// Remove namespace data from an xml element.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        /// <remarks><see cref="https://stackoverflow.com/questions/987135/how-to-remove-all-namespaces-from-xml-with-c"/> Konrad Morawski post 28-Sep-2011.</remarks>
        public static XmlElement RemoveNamespaces(this XmlElement element)
        {
            // Create new element without namespace.
            XmlElement stripped = element.OwnerDocument.CreateElement(element.Prefix, element.LocalName, string.Empty);

            // Add the attributes back.
            foreach (XmlAttribute attr in element.Attributes)
            {
                XmlAttribute cleanAttr = element.OwnerDocument.CreateAttribute(attr.Prefix, attr.LocalName, string.Empty);
                cleanAttr.Value = attr.Value;
                stripped.Attributes.Append(cleanAttr);
            }

            // Add childnodes back.
            if (element.HasChildNodes && element.Value == null)
            {
                for (int i = 0; i < element.ChildNodes.Count; i++)
                {
                    XmlNode node = element.ChildNodes[i];

                    if (node.NodeType == XmlNodeType.Element)
                    {
                        XmlElement ele = node.RemoveNamespaces();

                        if (ele != null)
                        {
                            stripped.AppendChild(ele);
                        }
                        else
                        {
                            stripped.InnerText = element.InnerText;
                            break;
                        }
                    }
                    else
                    {
                        stripped.AppendChild(node);
                    }
                }
            }
            else
            {
                stripped.InnerText = element.InnerText;
            }

            element = stripped;

            return stripped;
        }

        /// <summary>
        /// Remove the namespaces from an xml node.
        /// Note: the node must be an element type.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static XmlElement RemoveNamespaces(this XmlNode node)
        {
            XmlElement element = node.ToXmlElement();

            if (element != null)
            {
                element = element.RemoveNamespaces();
            }

            return element;
        }

        /// <summary>
        /// Converts a XmlNode of Element type to XmlElement.
        /// Other types of node are not converted and null is returned.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static XmlElement ToXmlElement(this XmlNode node)
        {
            if (node.NodeType == XmlNodeType.Element)
            {
                XmlElement result = node.OwnerDocument.CreateElement(node.Prefix, node.LocalName, node.NamespaceURI);

                foreach (XmlAttribute attr in node.Attributes)
                {
                    XmlAttribute cleanAttr = node.OwnerDocument.CreateAttribute(attr.Prefix, attr.LocalName, attr.NamespaceURI);
                    cleanAttr.Value = attr.Value;
                    result.Attributes.Append(cleanAttr);
                }

                if (node.HasChildNodes && node.Value == null)
                {
                    for (int i = 0; i < node.ChildNodes.Count; i++)
                    {
                        XmlNode childNode = node.ChildNodes[i];

                        if (node.NodeType == XmlNodeType.Element)
                        {
                            XmlElement ele = childNode.ToXmlElement();

                            if (ele != null)
                            {
                                result.AppendChild(childNode.ToXmlElement());
                            }
                            else
                            {
                                result.InnerText = node.InnerText;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    result.InnerText = node.InnerText;
                }

                node = result;

                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
