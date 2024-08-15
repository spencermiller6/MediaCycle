using System.ServiceModel.Syndication;
using System.Xml;
using MediaCycle.Core.ConfigurableFile;

namespace MediaCycle.Core
{
    public class RssChannel : DirectoryItem
    {
        private SyndicationFeed _feed;
        public string Url;
        public DateTime? LastFetch;

        public RssChannel(string name, string url) : base(name)
        {
            Url = url;
            _feed = new SyndicationFeed(Name, "", new Uri(Url));
        }

        public SyndicationFeed Feed()
        {
            if (LastFetch is null || LastFetch < ReleaseTime.NextReleaseTime())
            {
                FetchRssFeed();
            }

            return _feed;
        }

        public void FetchRssFeed()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(Url).Result;
                response.EnsureSuccessStatusCode();

                string responseBody = response.Content.ReadAsStringAsync().Result;
                using (XmlReader reader = XmlReader.Create(new System.IO.StringReader(responseBody)))
                {
                    LastFetch = DateTime.Now;
                    _feed = SyndicationFeed.Load(reader);
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
        public List<RssFolder> Folders;
        public List<RssChannel> Channels;
        private static RssFolder? _root;
        
        public RssFolder(string name) : base(name)
        {
            Folders = new List<RssFolder>();
            Channels = new List<RssChannel>();
        }

        public static RssFolder Root()
        {
            if (_root == null)
            {
                _root = Opml.ParseToDirectoryItems(Config.Instance().SubscriptionsFilePath);
            }

            return _root;
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
