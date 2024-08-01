using Core.Rss;

public class Program
{
    static void Main()
    {
        string path = "RSS.xml";

        RssFolder rootFolder = Core.Rss.Opml.ParseToDirectoryItems(path);
        rootFolder.Show();
    }

    public static List<DateTime> ReleaseTimes = new List<DateTime>
    {
        new DateTime(1970, 1, 1, 8, 0, 0),
        new DateTime(1970, 1, 1, 17, 0, 0)
    };

    public static DateTime? NextReleaseTime()
    {
        if (Program.ReleaseTimes.Count == 0)
        {
            return null;
        }

        DateTime currentTime = DateTime.Now;
        DateTime nextReleaseTime = new DateTime
        (
            currentTime.Year,
            currentTime.Month,
            currentTime.Day,
            ReleaseTimes[0].Hour,
            ReleaseTimes[0].Minute,
            0,
            0,
            currentTime.Kind
        ).AddDays(1);

        for (int i = Program.ReleaseTimes.Count - 1; i >= 0; i--)
        {
            DateTime releaseTime = new DateTime
            (
                currentTime.Year,
                currentTime.Month,
                currentTime.Day,
                ReleaseTimes[i].Hour,
                ReleaseTimes[i].Minute,
                0,
                0,
                currentTime.Kind
            );

            if (releaseTime < currentTime)
            {
                break;
            }
            else
            {
                nextReleaseTime = releaseTime;
            }
        }

        return nextReleaseTime;
    }
}
