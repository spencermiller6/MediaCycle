using System.Xml.Linq;
using static MediaCycle.Core.ReleaseTime;

namespace MediaCycle.Core
{
    public class ConfigManager
    {
        private static ConfigManager? _instance;
        public string SubscriptionsFilePath { get; private set; }
        public List<ReleaseTime> ReleaseTimes { get; private set; }

        private ConfigManager()
        {
            XDocument configFile = GetConfig();
            
            SubscriptionsFilePath = LoadSubscriptionsFilePath(configFile);
            ReleaseTimes = LoadReleaseTimes(configFile);
        }

        public static ConfigManager Instance()
        {
            if (_instance == null)
            {
                _instance = new ConfigManager();
            }
            return _instance;
        }

        private static XDocument GetConfig()
        {
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config/mediacycle/config.xml");

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Config file not found.", filePath);
            }

            return XDocument.Load(filePath);
        }

        public static string LoadSubscriptionsFilePath(XDocument doc)
        {
            return doc.Root.Element("SubscriptionsFilePath").Value;
        }

        public static List<ReleaseTime> LoadReleaseTimes(XDocument doc)
        {
            return doc.Root.Element("ReleaseTimes")
                .Elements("ReleaseTime")
                .Select(x => new ReleaseTime(
                    (DaysOfWeek)Enum.Parse(typeof(DaysOfWeek), (string)x.Element("Days")),
                    (int)x.Element("Hour"),
                    (int)x.Element("Minute")
                )).ToList();
        }
    }
}
