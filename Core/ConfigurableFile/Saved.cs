using System.Xml.Linq;

namespace MediaCycle.Core.ConfigurableFile
{
    public class Saved : ConfigurableFile
    {
        private const string _filename = "saved.xml";

        public XDocument LoadOrCreate()
        {
            string filePath = Path.Combine(ConfigurableFile.Directory, _filename);

            if (!File.Exists(filePath))
            {
                XDocument doc = new XDocument(new XElement("Articles"));
                doc.Save(_filename);
            }

            return XDocument.Load(filePath);
        }

        public static void SaveArticle(string articleId)
        {
            string filePath = Path.Combine(ConfigurableFile.Directory, _filename);
            XDocument doc = XDocument.Load(filePath);

            if (!doc.Descendants("Article").Any(e => e.Value == articleId))
            {
                doc.Element("Articles").Add(new XElement("Article", articleId));
                doc.Save(filePath);
            }
        }

        public static void RemoveArticle(string articleId)
        {
            string filePath = Path.Combine(ConfigurableFile.Directory, _filename);
            XDocument doc = XDocument.Load(filePath);
            XElement articleElement = doc.Descendants("Article").FirstOrDefault(e => e.Value == articleId);
            
            if (articleElement != null)
            {
                articleElement.Remove();
                doc.Save(filePath);
            }
        }
    }
}