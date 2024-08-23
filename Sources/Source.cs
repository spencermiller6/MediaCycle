using MediaCycle.Sync;

namespace MediaCycle.Sources;

public static class Source
{
    public static ISource GetSource(string sourceName)
    {
        switch(sourceName)
        {
            case "youtube":
                return new Youtube();
            default:
                throw new Exception("Invalid argument");
        }
    }
}
