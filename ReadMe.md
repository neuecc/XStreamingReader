XStreamingReader - Streaming for Linq to Xml
===
Xml Stream(XmlReader) to IEnumerable<XElement> for Windows Phone(memory save) or parse large Xml.

Features
---

* lazy evaluation and streaming parse
* same api as XElement(Load/Parse/Descendants/Elements/Attributes)
* extras:T4 Template - Xml to Class Auto Generate

Description
---
source is very simple. for example, Descendants

```csharp
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
```

Sample
---

```csharp
// Read Rss
var rss = XStreamingReader.Load("http://services.social.microsoft.com/feeds/feed/CSharpHeadlines")
    .Descendants("item")
    .Select(x => new
    {
        Title = (string)x.Element("title"),
        Description = (string)x.Element("description"),
        PubDate = (DateTime)x.Element("pubDate")
    })
    .ToArray();
```

*extras: T4 Template*
generate from xml to class.

import ClassGenerator.cs and change 3 strings.

```csharp
string XmlString = new WebClient().DownloadString("http://twitter.com/statuses/public_timeline.xml");
const string DescendantsName = "status"; // select class root
const string Namespace = "Twitter"; // namespace
```

Ctrl+S then Generate this Class

```csharp
namespace Twitter
{
    public class Status
    {
        public string CreatedAt { get; set; }
        public string Id { get; set; }
        // snip...
        public User User { get; set; }
        public string Geo { get; set; }

        public Status(XElement element)
        {
            this.CreatedAt = (string)element.Element("created_at");
            this.Id = (string)element.Element("id");
            this.User = new User(element.Element("user"));
            this.Geo = (string)element.Element("geo");
        }
    }
    
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ScreenName { get; set; }
        // snip...
        public string FollowRequestSent { get; set; }

        public User(XElement element)
        {
            this.Id = (string)element.Element("id");
            this.Name = (string)element.Element("name");
            this.ScreenName = (string)element.Element("screen_name");
            this.FollowRequestSent = (string)element.Element("follow_request_sent");
        }
    }
}
```

all type is string, copy and paste generated .cs file and rename and replace datatype manually.
