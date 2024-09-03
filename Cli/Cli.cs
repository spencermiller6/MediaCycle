using System.ServiceModel.Syndication;
using MediaCycle.Core;

namespace MediaCycle.Cli;

public static class Cli
{
    public static string[] Completions
    {
        get
        {
            if (_completions is null)
            {
                _completions = ChannelDictionary.Keys.ToArray();
            }

            return _completions;
        }
    }

    public static Dictionary<string, RssChannel> ChannelDictionary
    {
        get
        {
            if (!_folderDictionary.Any())
            {
                AddToChannelDictionary(RssFolder.Root, RssFolder.Root.ToPath());
            }

            return _folderDictionary;
        }
    }

    private static void AddToChannelDictionary(RssFolder parentFolder, string parentPath)
    {
        foreach (RssFolder folder in parentFolder.Folders)
        {
            string childPath = Path.Combine(parentPath, folder.Name);
            AddToChannelDictionary(folder, childPath);
        }

        foreach (RssChannel channel in parentFolder.Channels)
        {
            string childPath = Path.Combine(parentPath, channel.Name);
            _folderDictionary.Add(childPath, channel);
        }
    }

    private static string[]? _completions;
    private static Dictionary<string, RssChannel> _folderDictionary = new Dictionary<string, RssChannel>();
    public static List<SyndicationItem>? PresentFeed;
}
