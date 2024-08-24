using MediaCycle.Sync;

namespace MediaCycle.Sources;

public static class SourceFactory
{
    public static ISource BuildSource(string sourceName)
    {
        switch(sourceName)
        {
            case "youtube":
                return new Youtube();
            default:
                throw new Exception("Invalid argument");
        }
    }

    public static IEnumerable<ISource> BuildAllSources()
    {
        yield return new Youtube();
    }
}
