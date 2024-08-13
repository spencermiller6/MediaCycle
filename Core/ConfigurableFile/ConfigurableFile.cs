using System.Xml.Linq;

namespace MediaCycle.Core.ConfigurableFile
{
    public interface ConfigurableFile
    {
        public static string Directory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config/mediacycle/");
        public abstract XDocument LoadOrCreate();
    }
}
