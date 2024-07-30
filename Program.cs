using Core.Rss;

class Program
{
    static void Main()
    {
        string path = "RSS.xml";
        RssFolder rootFolder = Core.Rss.Opml.ParseToDirectoryItems(path);
    }
}
