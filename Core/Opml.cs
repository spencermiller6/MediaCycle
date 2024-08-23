using System.Xml;
using MediaCycle.Core.ConfigurableFile;

namespace MediaCycle.Core
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
                        // Folder
                        RssFolder subFolder = new RssFolder(title)
                        {
                            Parent = folder
                        };

                        folder.Folders.Add(subFolder);
                        ParseOutline(childNode, subFolder);
                    }
                    else
                    {
                        // Channel
                        RssChannel channel = new RssChannel(title, xmlUrl)
                        {
                            Parent = folder
                        };
                        
                        folder.Channels.Add(channel);
                    }
                }
            }
        }

        public static void OverwriteXml(string title, List<RssChannel> rssChannels)
        {
            string fileName = Path.ChangeExtension(title.ToLower(), "xml");
            string filePath = Path.Combine(Config.Instance().SubscriptionsFilePath, fileName);
            using (XmlWriter writer = XmlWriter.Create(filePath, new XmlWriterSettings { Indent = true }))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("opml");
                writer.WriteAttributeString("version", "1.0");

                writer.WriteStartElement("head");
                writer.WriteElementString("title", title);
                writer.WriteEndElement();

                writer.WriteStartElement("body");
                writer.WriteStartElement("outline");

                foreach (RssChannel channel in rssChannels)
                {
                    writer.WriteStartElement("outline");
                    writer.WriteAttributeString("text", channel.Name);
                    writer.WriteAttributeString("type", "rss");
                    writer.WriteAttributeString("xmlUrl", channel.Url);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
    }
}