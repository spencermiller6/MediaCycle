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

public class ReleaseTime
{
    [Flags]
    public enum DaysOfWeek
    {
        None = 0,
        Sunday = 1 << 0,
        Monday = 1 << 1,
        Tuesday = 1 << 2,
        Wednesday = 1 << 3,
        Thursday = 1 << 4,
        Friday = 1 << 5,
        Saturday = 1 << 6,
        Weekdays = Monday | Tuesday | Wednesday | Thursday | Friday,
        Weekend = Saturday | Sunday,
        EveryDay = Weekdays | Weekend
    }

    public DaysOfWeek Days;
    public int Hour;
    public int Minute;

    public ReleaseTime(DaysOfWeek days, int hour, int minute)
    {
        Days = days;
        Hour = hour;
        Minute = minute;
    }

    public DateTime OnDate(DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day, Hour, Minute, 0, 0, date.Kind);
    }

    public static DateTime? NextReleaseTime()
    {
        if (Program.ReleaseTimes.Count == 0)
        {
            return null;
        }

        DateTime today = DateTime.Now;
        DateTime nextReleaseTime = Program.ReleaseTimes[0].OnDate(today).AddDays(1);

        for (int i = Program.ReleaseTimes.Count - 1; i >= 0; i--)
        {
            DateTime releaseTime = Program.ReleaseTimes[i].OnDate(today);

            if (releaseTime < today)
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