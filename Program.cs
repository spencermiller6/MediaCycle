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

    public DateTime NextOccurrence(DateTime currentTime)
    {
        DateTime todaysOccurance = new DateTime
        (
            currentTime.Year,
            currentTime.Month,
            currentTime.Day,
            Program.ReleaseTimes[0].Hour,
            Program.ReleaseTimes[0].Minute,
            0,
            0,
            currentTime.Kind
        );

        return todaysOccurance > currentTime ? todaysOccurance : todaysOccurance.AddDays(1);
    }

    public DateTime NextOccurrence() => NextOccurrence(DateTime.Now);

    public static DateTime? NextReleaseTime()
    {
        if (Program.ReleaseTimes.Count == 0)
        {
            return null;
        }

        DateTime currentTime = DateTime.Now;
        DateTime nextReleaseTime = Program.ReleaseTimes.Last().NextOccurrence(currentTime);

        for (int i = Program.ReleaseTimes.Count - 2; i >= 0; i--)
        {
            DateTime releaseTime = Program.ReleaseTimes[i].NextOccurrence(currentTime);

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