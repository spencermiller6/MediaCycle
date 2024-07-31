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
                        // Folder
                        RssFolder subFolder = new RssFolder(title);
                        folder.Children.Add(subFolder);
                        ParseOutline(childNode, subFolder);
                    }
                    else
                    {
                        // Channel
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
        public SyndicationFeed Feed;

        public RssChannel(string name, string url) : base(name)
        {
            Url = url;
            Feed = new SyndicationFeed(Name, "", new Uri(Url));
        }

        public static void FetchRssFeed(string url)
        {        
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = client.GetAsync(url).Result;
                    response.EnsureSuccessStatusCode();

                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    using (XmlReader reader = XmlReader.Create(new System.IO.StringReader(responseBody)))
                    {
                        SyndicationFeed feed = SyndicationFeed.Load(reader);
                        Console.WriteLine($"Title: {feed.Title.Text}");

                        foreach (SyndicationItem item in feed.Items)
                        {
                            Console.WriteLine($"Title: {item.Title.Text}");
                            Console.WriteLine($"Published Date: {item.PublishDate}");
                            Console.WriteLine($"Summary: {item.Summary.Text}");
                            Console.WriteLine($"Link: {item.Id}");
                            Console.WriteLine($"Author: {RssChannel.GetAuthors(item)}");

                            Console.WriteLine();
                        }
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Request error: {e.Message}");
                }
                catch (XmlException e)
                {
                    Console.WriteLine($"XML error: {e.Message}");
                }
            }
        }

        public static string GetAuthors(SyndicationItem item)
        {
            switch (item.Authors.Count())
            {
                case 0: return "";
                case 1: return item.Authors[0].Name;
                default: return "Multiple authors";
            }
        }
    }

    public class RssFolder : DirectoryItem
    {
        public List<DirectoryItem> Children; 
        
        public RssFolder(string name) : base(name)
        {
            Children = new List<DirectoryItem>();
        }

        public void Show()
        {
            int index = 0;

            foreach (DirectoryItem child in Children)
            {
                Console.WriteLine($"{index++}\t{child.Name}");
            }
            
            try
            {
                int selection = int.Parse(Console.ReadLine());
                if (selection >=0 && selection < Children.Count)
                {
                    if (Children[selection] is RssFolder directory)
                    {
                        directory.Show();
                    }
                    else if (Children[selection] is RssChannel channel)
                    {
                        RssChannel.FetchRssFeed(channel.Url);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("You must provide a valid selection.");
            }
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