using System.Drawing.Text;
using System.ServiceModel.Syndication;
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

                        folder.Children.Add(subFolder);
                        ParseOutline(childNode, subFolder);
                    }
                    else
                    {
                        // Channel
                        RssChannel channel = new RssChannel(title, xmlUrl)
                        {
                            Parent = folder
                        };
                        
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

        public static SyndicationFeed FetchRssFeed(string url)
        {        
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();

                string responseBody = response.Content.ReadAsStringAsync().Result;
                using (XmlReader reader = XmlReader.Create(new System.IO.StringReader(responseBody)))
                {
                    return SyndicationFeed.Load(reader);
                }
            }
        }

        public void Show()
        {
            if (Feed is null)
            {
                Feed = FetchRssFeed(Url);
            }

            DateTime? releaseTime = ReleaseTime.NextReleaseTime();

            Console.WriteLine($"{-1}\t<- {Parent.Name}");
            Console.WriteLine($"Title: {Feed.Title.Text}");
            Console.WriteLine($"Next Release Time: {releaseTime}");

            int index = 0;

            foreach (SyndicationItem item in Feed.Items)
            {
                if (item.PublishDate > releaseTime)
                {
                    break;
                }

                Console.WriteLine(index++);
                Console.WriteLine($"Title: {item.Title.Text}");
                Console.WriteLine($"Published Date: {item.PublishDate}");
                Console.WriteLine($"Summary: {item.Summary.Text}");
                Console.WriteLine($"Link: {item.Id}");
                Console.WriteLine($"Author: {RssChannel.GetAuthors(item)}");
                Console.WriteLine();
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
        private static RssFolder? _root;
        
        public RssFolder(string name) : base(name)
        {
            Children = new List<DirectoryItem>();
        }

        public static RssFolder Root()
        {
            if (_root == null)
            {
                _root = Opml.ParseToDirectoryItems(Config.Instance().SubscriptionsFilePath);
            }

            return _root;
        }

        public void Show()
        {
            int index = 0;

            if (Parent is not null)
            {
                Console.WriteLine($"{-1}\t<- {Parent.Name}");
            }

            foreach (DirectoryItem child in Children)
            {
                Console.WriteLine($"{index++}\t{child.Name}");
            }
            
            try
            {
                int selection = int.Parse(Console.ReadLine());

                if (selection == -1)
                {
                    Parent.Show();
                }
                if (selection >= 0 && selection < Children.Count)
                {
                    if (Children[selection] is RssFolder directory)
                    {
                        directory.Show();
                    }
                    else if (Children[selection] is RssChannel channel)
                    {
                        channel.Show();
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
        public RssFolder? Parent;

        public DirectoryItem(string name)
        {
            Name = name;
        }
    }
}