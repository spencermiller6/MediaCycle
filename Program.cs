using Core.Rss;

public class Program
{
    static void Main()
    {
        string path = "RSS.xml";

        RssFolder rootFolder = Core.Rss.Opml.ParseToDirectoryItems(path);
        rootFolder.Show();
    }

    public static List<ReleaseTime> ReleaseTimes = new List<ReleaseTime>
    {
        new ReleaseTime(ReleaseTime.DaysOfWeek.EveryDay, 8, 0),
        new ReleaseTime(ReleaseTime.DaysOfWeek.EveryDay, 17, 0)
    };
}
