using System.Xml;

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
    }
}