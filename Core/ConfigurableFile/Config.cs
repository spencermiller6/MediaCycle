using System.Xml.Linq;
using static MediaCycle.Core.ReleaseTime;

namespace MediaCycle.Core.ConfigurableFile
{
    public class Config : ConfigurableFile
    {
        private static Config? _instance;
        public string SubscriptionsFilePath { get; private set; }
        public List<ReleaseTime> ReleaseTimes { get; private set; }

        private Config()
        {
            XDocument configFile = LoadOrCreate();
            SubscriptionsFilePath = LoadSourcesFilePath(configFile);
            ReleaseTimes = LoadReleaseTimes(configFile);
        }

        public XDocument LoadOrCreate()
        {
            string filePath = Path.Combine(ConfigurableFile.Directory, "config.xml");
            XDocument configDoc;

            if (File.Exists(filePath))
            {
                configDoc = XDocument.Load(filePath);
            }
            else
            {
                configDoc = new XDocument(new XElement("Config"));
            }

            if (configDoc.Root.Element("ReleaseTimes") == null)
            {
                configDoc.Root.Add(new XElement("ReleaseTimes"));
            }

            configDoc.Save(filePath);
            return XDocument.Load(filePath);
        }

        public static Config Instance()
        {
            if (_instance == null)
            {
                _instance = new Config();
            }

            return _instance;
        }

        public static string LoadSourcesFilePath(XDocument doc)
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
