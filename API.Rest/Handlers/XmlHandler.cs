using System.Globalization;
using System.Text;
using System.Buffers;
using System.Xml;
using System.Xml.Serialization;

using DotNext.Buffers;
using DotNext.IO;

namespace Handlers;

public static class XmlHandler
{
    public static XmlDocument SerializeObjectToXmlDocument(object? obj, string namespaceStr = "")
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        XmlDocument doc = new XmlDocument();
        XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
        ns.Add(String.Empty, namespaceStr);

        using (var ar = new PooledArrayBufferWriter<byte> { BufferPool = ArrayPool<byte>.Shared, Capacity = 2048 })
        {
            using (var memoryStream = ar.AsStream())
            {
                try
                {
                    xmlSerializer.Serialize(memoryStream, obj, ns);
                }
                catch (InvalidOperationException)
                {
                    return doc;
                }
                doc = new XmlDocument();
                doc.Load(ar.WrittenMemory.AsStream());
            }
        }

        return doc;
    }

    public static T DeserializeResponse<T>(XmlDocument resp)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        using (XmlReader reader = new XmlNodeReader(resp))
        {
            return (T)serializer.Deserialize(reader);
        }
    }

    public static string ValidateRequest(XmlDocument request, string requestNodeName)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        string? nspace = GetRequestNamespace(request, requestNodeName);

        if (string.IsNullOrEmpty(nspace))
        {
            throw new InvalidOperationException(
                "requestMessage is missing in the XML document.");
        }

        return nspace;
    }

    public static XmlNode? GetNode(XmlDocument doc, string tagName, string nspace)
    {
        XmlNodeList? nodes = doc?.GetElementsByTagName(tagName, nspace);

        if (nodes is not null && nodes.Count > 0)
        {
            return (nodes[0]);
        }

        return null;
    }

    public static XmlNode? GetNode(XmlDocument doc, string tagName)
    {
        XmlNodeList? nodes
            = doc?.GetElementsByTagName(tagName);

        if (nodes is not null && nodes.Count > 0)
        {
            return (nodes[0]);
        }

        return null;
    }

    private static string? GetRequestNamespace(XmlDocument request, string requestNodeName)
    {
        if (request is null)
        {
            return null;
        }

        XmlNodeList list = request.ChildNodes;
        foreach (XmlNode? node in list)
        {
            if (node is null)
            {
                continue;
            }

            if (StringsHandler.Equals(requestNodeName, node.LocalName))
            {
                if (string.IsNullOrEmpty(node.NamespaceURI))
                {
                    var childNode = node.ChildNodes[0];
                    return childNode.NamespaceURI;
                }

                return node.NamespaceURI;
            }
        }

        return null;
    }

    public static void SetField(
        XmlNode parentNode, ref XmlNode? previousSibling,
        string tagName, string tagValue, string nspace)
    {
        if (parentNode is null)
        {
            throw new ArgumentNullException(nameof(parentNode));
        }

        // obtain a pointer to the XmlDocument object.  This is used
        // in a few places in this method.
        XmlDocument doc = parentNode!.OwnerDocument;

        // create an XmlText object to hold the field's value.
        XmlText text = doc.CreateTextNode(tagValue);

        // look for the field.
        XmlNode? node = GetNode(doc, tagName, nspace);

        // if the field does not exist,...
        if (node is null)
        {
            // create an element for it and inside this element,
            // insert the XmlText object we created earlier.
            node = doc.CreateElement(tagName, nspace);
            node.AppendChild(text);

            // if there is a previous sibling, insert the new node
            // after it.
            if (previousSibling is not null)
            {
                parentNode.InsertAfter(node, previousSibling);
            }
            // else, the new node becomes the first child.
            else
            {
                parentNode.PrependChild(node);
            }
        }
        // else, if the field already exists, replace its value.
        else
        {
            // if the field does have a value, replace it with
            // the XmlText object we created earlier.
            if (node.HasChildNodes)
            {
                node.ReplaceChild(text, node.ChildNodes[0]);
            }
            // else, if it's empty, append the XmlText object
            // we created earlier.
            else
            {
                node.AppendChild(text);
            }
        }

        // the next node to be added will be after this node.  So, we
        // set previousSibling to this node.
        previousSibling = node;
    }
}