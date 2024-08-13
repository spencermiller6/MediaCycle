using MediaCycle.Core;
using MediaCycle.Core.ConfigurableFile;

public class Program
{
    static void Main()
    {
        RssFolder rootFolder = Opml.ParseToDirectoryItems(Config.Instance().SubscriptionsFilePath);
        rootFolder.Show();
    }
}
