/*--------------------------------------------------------------------------
* XStreamingReader
* ver 1.0.0.0 (Jul. 15th, 2010)
*
* created and maintained by neuecc <ils@neue.cc>
* licensed under Microsoft Public License(Ms-PL)
* http://neue.cc/
* http://xstreamingreader.codeplex.com/
*--------------------------------------------------------------------------*/

// InstallGuide

include XStreamingReader.cs

// HowToUse

// create XStreamingReader
XStreamingReader.Load / XStreamingReader.Parse
// selector methods
Attribute/Attributes/Element/Elements/Descendants

// example read rss
var rss = XStreamingReader.Load("http://services.social.microsoft.com/feeds/feed/CSharpHeadlines")
    .Descendants("item")
    .Select(x => new
    {
        Title = (string)x.Element("title"),
        Description = (string)x.Element("description"),
        PubDate = (DateTime)x.Element("pubDate")
    })
    .ToArray();

// HowToUse:T4 Template - ClassGenerator.tt

include ClassGenerator.tt on your project.
replace three variable
XmlString, DescendantsName, Namespace.

Ctrl+S -> Class Generated!
open ClassGenerator.cs and Ctrl+A -> Ctrl+C(all copy)
create new .cs file and paste(Ctrl+V)
finally change data type manually(default is all string)

// History

2010-07-15 ver 1.0.0.0
1st Release