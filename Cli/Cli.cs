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
                _completions = FolderDictionary.Keys.ToArray();
            }

            return _completions;
        }
    }

    public static Dictionary<string, DirectoryItem> FolderDictionary
    {
        get
        {
            if (!_folderDictionary.Any())
            {
                CreatePathDictionaries(RssFolder.Root, RssFolder.Root.ToPath());
            }

            return _folderDictionary;
        }
    }

    private static void CreatePathDictionaries(RssFolder parentFolder, string parentPath)
    {
        _folderDictionary.Add(parentPath, parentFolder);

        foreach (RssFolder folder in parentFolder.Folders)
        {
            string childPath = Path.Combine(parentPath, folder.Name);
            CreatePathDictionaries(folder, childPath);
        }

        foreach (RssChannel channel in parentFolder.Channels)
        {
            string childPath = Path.Combine(parentPath, channel.Name);
            _folderDictionary.Add(childPath, channel);
        }
    }

    private static string[]? _completions;
    private static Dictionary<string, DirectoryItem> _folderDictionary = new Dictionary<string, DirectoryItem>();
    public static List<SyndicationItem>? PresentFeed;
}
