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

        private static DateTime? _nextReleaseTime;
        private static List<DateTime> _pastReleaseTimes;

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
            _pastReleaseTimes = new List<DateTime>();
        }

        public DateTime OnDate(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, Hour, Minute, 0, 0, date.Kind);
        }

        public DateTime NextReleaseTime(DateTime lowerBound)
        {
            if (_nextReleaseTime is null || _nextReleaseTime < DateTime.Now)
            {
                DateTime? nextReleaseTime = null;

                for (int i = 0; i < Config.Instance().ReleaseTimes.Count; i++)
                {
                    DateTime releaseTime = Config.Instance().ReleaseTimes[i].OnDate(lowerBound);

                    if (releaseTime > lowerBound && (nextReleaseTime is null || releaseTime > nextReleaseTime))
                    {
                        nextReleaseTime = releaseTime;
                    }
                }

                if (nextReleaseTime is null)
                {
                    NextReleaseTime(lowerBound.AddDays(1));
                }
                
                _nextReleaseTime = nextReleaseTime;
            }
            
            return (DateTime)_nextReleaseTime;
        }

        public DateTime PreviousReleaseTime
        {

        }

        // I need to make two methods, NextReleaseTime and PreviousReleaseTime, that take in an int offset to get a particular index
        // Or maybe have a list of previous ones that I prepend to, and a singular one for next
        // They also have to check every single releasetime in config
        public static DateTime? NextReleaseTime()
        {
            if (Config.Instance().ReleaseTimes.Count == 0)
            {
                return null;
            }

            DateTime today = DateTime.Now;
            DateTime nextReleaseTime = Config.Instance().ReleaseTimes[0].OnDate(today).AddDays(1);

            for (int i = Config.Instance().ReleaseTimes.Count - 1; i >= 0; i--)
            {
                DateTime releaseTime = Config.Instance().ReleaseTimes[i].OnDate(today);

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
}
