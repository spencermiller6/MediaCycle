using MediaCycle.Core;

public class Program
{
    static void Main()
    {
        RssFolder rootFolder = Opml.ParseToDirectoryItems(ConfigManager.Instance().SubscriptionsFilePath);
        rootFolder.Show();
    }
}
