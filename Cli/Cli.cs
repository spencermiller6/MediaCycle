using MediaCycle.Core;

namespace MediaCycle.Cli;

public static class Cli
{
    public static RssFolder Pwd()
    {
        if (_pwd is null)
        {
            _pwd = RssFolder.Root();
        }

        return _pwd;
    }

    private static RssFolder? _pwd;
}
