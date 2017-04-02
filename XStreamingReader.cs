/*--------------------------------------------------------------------------
* XStreamingReader
* ver 1.0.0.0 (Jul. 15th, 2010)
*
* created and maintained by neuecc <ils@neue.cc>
* licensed under Microsoft Public License(Ms-PL)
* https://github.com/neuecc/XStreamingReader/
*--------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace System.Xml.Linq
{
    public class XStreamingReader
    {
        // static

        public static XStreamingReader Load(Stream stream)
        {
            return new XStreamingReader(() => XmlReader.Create(stream));
        }

        public static XStreamingReader Load(string uri)
        {
            return new XStreamingReader(() => XmlReader.Create(uri));
        }

        public static XStreamingReader Load(TextReader textReader)
        {
            return new XStreamingReader(() => XmlReader.Create(textReader));
        }

        public static XStreamingReader Load(XmlReader reader)
        {
            return new XStreamingReader(() => reader);
        }

        public static XStreamingReader Parse(string text)
        {
            return new XStreamingReader(() => XmlReader.Create(new StringReader(text)));
        }

        // instance

        readonly Func<XmlReader> readerFactory;

        private XStreamingReader(Func<XmlReader> readerFactory)
        {
            this.readerFactory = readerFactory;
        }

        void MoveToNextElement(XmlReader reader)
        {
            while (reader.Read() && reader.NodeType != XmlNodeType.Element) { }
        }

        void MoveToNextFollowing(XmlReader reader)
        {
            var depth = reader.Depth;
            if (reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
            {
                while (reader.Read() && depth < reader.Depth) { }
            }
            MoveToNextElement(reader);
        }

        public XAttribute Attribute(XName name)
        {
            return Attributes(name).FirstOrDefault();
        }

        public IEnumerable<XAttribute> Attributes(XName name)
        {
            return Attributes().Where(x => x.Name == name);
        }

        public IEnumerable<XAttribute> Attributes()
        {
            using (var reader = readerFactory())
            {
                reader.MoveToContent();
                while (reader.MoveToNextAttribute())
                {
                    XNamespace ns = reader.NamespaceURI;
                    XName name = ns + reader.Name.Split(':').Last();
                    yield return new XAttribute(name, reader.Value);
                }
            }
        }

        public XElement Element(XName name)
        {
            return Elements(name).FirstOrDefault();
        }

        public IEnumerable<XElement> Elements()
        {
            using (var reader = readerFactory())
            {
                reader.MoveToContent();
                MoveToNextElement(reader);
                while (!reader.EOF)
                {
                    yield return XElement.Load(reader.ReadSubtree());
                    MoveToNextFollowing(reader);
                }
            }
        }

        public IEnumerable<XElement> Elements(XName name)
        {
            return Elements().Where(x => x.Name == name);
        }

        public IEnumerable<XElement> Descendants(XName name)
        {
            using (var reader = readerFactory())
            {
                while (reader.ReadToFollowing(name.LocalName, name.NamespaceName))
                {
                    yield return XElement.Load(reader.ReadSubtree());
                }
            }
        }
    }
}
