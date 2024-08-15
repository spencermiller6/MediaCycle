using MediaCycle.Core.ConfigurableFile;

namespace MediaCycle.Core
{
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
        public string? Name; //To-Do

        // private static DateTime? _nextReleaseTime;
        private static List<DateTime> _pastReleaseTimes = new List<DateTime>();

        public ReleaseTime(DaysOfWeek days, int hour, int minute)
        {
            if (hour < 0 || hour > 23)
            {
                throw new ArgumentOutOfRangeException(nameof(hour), "Hour must be between 1 and 24.");
            }

            if (minute < 0 || minute > 59)
            {
                throw new ArgumentOutOfRangeException(nameof(minute), "Minute must be between 0 and 59.");
            }

            Days = days;
            Hour = hour;
            Minute = minute;
        }

        public static DaysOfWeek DaysOfWeekFromDate(DateTime dateTime)
        {
            return (DaysOfWeek)(1 << (int)dateTime.DayOfWeek);
        }

        public static bool AnyActiveReleaseTimes()
        {
            foreach (ReleaseTime releaseTime in Config.Instance().ReleaseTimes)
            {
                if (releaseTime.Days != DaysOfWeek.None)
                {
                    return true;
                }
            }

            return false;
        }

        public DateTime? OnDate(DateTime date)
        {
            DaysOfWeek dayOfWeek = DaysOfWeekFromDate(date);
            if ((dayOfWeek & Days) == dayOfWeek)
            {
                return new DateTime(date.Year, date.Month, date.Day, Hour, Minute, 0, 0, date.Kind);
            }

            return null;
        }

        public static DateTime EarliestReleaseTimeAfterDate(DateTime dateToCheck, DateTime lowerBound)
        {
            DateTime? nextReleaseTime = null;

            for (int i = 0; i < Config.Instance().ReleaseTimes.Count; i++)
            {
                DateTime? releaseTime = Config.Instance().ReleaseTimes[i].OnDate(dateToCheck);

                if (releaseTime > lowerBound && (nextReleaseTime is null || releaseTime < nextReleaseTime))
                {
                    nextReleaseTime = releaseTime;
                }
            }

            nextReleaseTime ??= EarliestReleaseTimeAfterDate(dateToCheck.AddDays(1), lowerBound);
            return (DateTime)nextReleaseTime;
        }

        public static DateTime EarliestReleaseTimeAfterDate(DateTime lowerBound) => LatestReleaseTimeBeforeDate(lowerBound, lowerBound);

        public static DateTime LatestReleaseTimeBeforeDate(DateTime dateToCheck, DateTime upperBound)
        {
            DateTime? previousReleaseTime = null;

            for (int i = 0; i < Config.Instance().ReleaseTimes.Count; i++)
            {
                DateTime? releaseTime = Config.Instance().ReleaseTimes[i].OnDate(dateToCheck);

                if (releaseTime < upperBound && (previousReleaseTime is null || releaseTime > previousReleaseTime))
                {
                    previousReleaseTime = releaseTime;
                }
            }

            previousReleaseTime ??= LatestReleaseTimeBeforeDate(dateToCheck.AddDays(-1), upperBound);
            return (DateTime)previousReleaseTime;
        }

        public static DateTime LatestReleaseTimeBeforeDate(DateTime upperBound) => LatestReleaseTimeBeforeDate(upperBound, upperBound);

        public static DateTime PastReleaseTimeAtIndex(int index)
        {
            DateTime previousReleaseTime = LatestReleaseTimeBeforeDate(DateTime.Now);

            if (_pastReleaseTimes.Count() == 0)
            {
                _pastReleaseTimes.Add(previousReleaseTime);
            }

            while (_pastReleaseTimes[0] != previousReleaseTime)
            {
                _pastReleaseTimes.Insert(0, EarliestReleaseTimeAfterDate(_pastReleaseTimes.First()));
            }

            while (index > _pastReleaseTimes.Count() - 1)
            {
                _pastReleaseTimes.Add(LatestReleaseTimeBeforeDate(_pastReleaseTimes.Last()));
            }

            return _pastReleaseTimes[index];
        }

        public static DateTime NextReleaseTime() => EarliestReleaseTimeAfterDate(DateTime.Now);
    }
    // To-Do: validate that there are active release times before assuming there are
}
