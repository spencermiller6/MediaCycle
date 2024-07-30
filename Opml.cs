using System.Drawing.Text;
using System.ServiceModel.Syndication;
using System.Xml;

namespace Core.Rss
{
    public static class Opml
    {
        public static RssFolder ParseToDirectoryItems(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode bodyNode = doc.SelectSingleNode("/opml/body");
            if (bodyNode == null)
            {
                throw new Exception("Invalid OPML file: missing body element.");
            }

            RssFolder rootFolder = new RssFolder("Root");
            ParseOutline(bodyNode, rootFolder);

            return rootFolder;
        }

        private static void ParseOutline(XmlNode node, RssFolder folder)
        {
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode.Name == "outline")
                {
                    string title = childNode.Attributes["title"]?.Value;
                    string xmlUrl = childNode.Attributes["xmlUrl"]?.Value;

                    if (string.IsNullOrEmpty(xmlUrl))
                    {
                        // It's a folder
                        RssFolder subFolder = new RssFolder(title);
                        folder.Children.Add(subFolder);
                        ParseOutline(childNode, subFolder);
                    }
                    else
                    {
                        // It's a channel
                        RssChannel channel = new RssChannel(title, xmlUrl);
                        folder.Children.Add(channel);
                    }
                }
            }
        }
    }

    public class RssChannel : DirectoryItem
    {
        public string Url;
        public SyndicationFeed syndicationFeed;

        public RssChannel(string name, string url) : base(name)
        {
            Url = url;
            syndicationFeed = new SyndicationFeed(Name, "", new Uri(Url));
        }
    }

    public class RssFolder : DirectoryItem
    {
        public List<DirectoryItem> Children; 
        
        public RssFolder(string name) : base(name)
        {
            Children = new List<DirectoryItem>();
        }
    }

    public abstract class DirectoryItem
    {
        public string Name;

        public DirectoryItem(string name)
        {
            Name = name;
        }
    }
}