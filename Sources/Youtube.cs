using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using MediaCycle.Core;
using MediaCycle.Sources;

namespace MediaCycle.Sync
{
    public class Youtube : ISource
    {
        static string[] Scopes = { YouTubeService.Scope.YoutubeReadonly };
        static string ApplicationName = "MediaCycle";
        
        private static UserCredential? _userCredential;

        public void Connect()
        {
            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                _userCredential = GoogleWebAuthorizationBroker.AuthorizeAsync
                (
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(ApplicationName, true)
                ).Result;
            }
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public static List<RssChannel> GetSubscriptions()
        {
            BaseClientService.Initializer initializer = new BaseClientService.Initializer()
            {
                HttpClientInitializer = _userCredential,
                ApplicationName = ApplicationName
            };

            string? nextPageToken = null;
            YouTubeService youTubeService = new YouTubeService(initializer);
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
            }
            while (!string.IsNullOrEmpty(nextPageToken));

            List<RssChannel> rssChannels = subscriptions.Select(s => new RssChannel
            (
                s.Snippet.Title,
                $"https://www.youtube.com/feeds/videos.xml?channel_id={s.Snippet.ResourceId.ChannelId}"
            )).ToList();

            return rssChannels;
        }

        public void Sync()
        {
            List<RssChannel> rssChannels = GetSubscriptions();
            Opml.OverwriteXml("youtube", rssChannels);
        }
    }
}
