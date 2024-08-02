using Core.Rss;
using ReleaseTime;

public class Program
{
    static void Main()
    {
        string path = "RSS.xml";

        RssFolder rootFolder = Core.Rss.Opml.ParseToDirectoryItems(path);
        rootFolder.Show();
    }

    public static List<ReleaseTime.ReleaseTime> ReleaseTimes = new List<ReleaseTime.ReleaseTime>
    {
        new ReleaseTime.ReleaseTime(ReleaseTime.ReleaseTime.DaysOfWeek.EveryDay, 8, 0),
        new ReleaseTime.ReleaseTime(ReleaseTime.ReleaseTime.DaysOfWeek.EveryDay, 17, 0)
    };
}
