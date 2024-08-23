using System.Xml;
using System.Xml.Linq;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using MediaCycle.Core.ConfigurableFile;

namespace MediaCycle.Sync
{
    public static class Youtube
    {
        static string[] Scopes = { YouTubeService.Scope.YoutubeReadonly };
        static string ApplicationName = "MediaCycle";

        public static void Execute()
        {
            UserCredential credential = GetCredential();
            YouTubeService youTubeService = GetYouTubeService(credential);
            List<Subscription> subscriptions = GetSubscriptions(youTubeService);
            AppendSubscriptionsToExistingRssXml(subscriptions);
        }

        public static UserCredential GetCredential()
        {
            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                return GoogleWebAuthorizationBroker.AuthorizeAsync
                (
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(ApplicationName, true)
                ).Result;
            }
        }

        public static YouTubeService GetYouTubeService(UserCredential credential)
        {
            BaseClientService.Initializer initializer = new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            };

            return new YouTubeService(initializer);
        }

        public static List<Subscription> GetSubscriptions(YouTubeService youTubeService)
        {
            string? nextPageToken = null;
            List<Subscription> subscriptions = new List<Subscription>();

            do
            {
                SubscriptionsResource.ListRequest request = youTubeService.Subscriptions.List("snippet");
                request.Mine = true;
                request.MaxResults = 50;
                request.PageToken = nextPageToken;

                SubscriptionListResponse response = request.Execute();
                subscriptions.AddRange(response.Items);
                nextPageToken = response.NextPageToken;
            } while (!string.IsNullOrEmpty(nextPageToken));
            
            return subscriptions;
        }

        static void AppendSubscriptionsToExistingRssXml(List<Subscription> subscriptions)
        {
            string filePath = Config.Instance().SubscriptionsFilePath;
            XDocument xmlDoc = XDocument.Load(filePath);
            XElement? youtubeOutline = xmlDoc.Descendants("outline").FirstOrDefault(e => (string)e.Attribute("text") == "YouTube");

            if (youtubeOutline != null)
            {
                foreach (Subscription subscription in subscriptions)
                {
                    XElement newOutline = new XElement("outline",
                        new XAttribute("text", subscription.Snippet.Title),
                        new XAttribute("title", subscription.Snippet.Title),
                        new XAttribute("type", "rss"),
                        new XAttribute("xmlUrl", $"https://www.youtube.com/feeds/videos.xml?channel_id={subscription.Snippet.ResourceId.ChannelId}"),
                        new XAttribute("htmlUrl", $"https://www.youtube.com/channel/{subscription.Snippet.ResourceId.ChannelId}")
                    );

                    youtubeOutline.Add(newOutline);
                }

                xmlDoc.Save(filePath);
            }
            else
            {
                Console.WriteLine("The specified <outline> element with text=\"YouTube\" and title=\"YouTube Feed\" was not found.");
            }
        }
    }
}
