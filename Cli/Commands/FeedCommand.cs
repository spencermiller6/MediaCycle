using System;
using System.Collections.Immutable;
using System.ServiceModel.Syndication;
using MediaCycle.Core;

namespace MediaCycle.Cli.Commands;

public class FeedCommand : Command
{
    public override string Name => "feed";
    public override string HelpText => "Show the feed of all channels within the current scope";
    public override int MinArguments => 0;
    public override int MaxArguments => 0;
    public override List<string> Arguments => _arguments;
    public override List<Option> Options => _options;

    private List<string> _arguments = new List<string>();
    private List<Option> _options = new List<Option>();

    public FeedCommand(List<string> arguments, List<char> shortOptions, List<string> longOptions) : base(arguments, shortOptions, longOptions)
    {
    }

    public override void Execute()
    {
        List<SyndicationItem> feed = GetFeedsFromFolder(Cli.Pwd);
        List<SyndicationItem> sortedFeed = feed.OrderByDescending(item => item.PublishDate).ToList();
        
        ShowFeed(sortedFeed);
    }
        
    static List<SyndicationItem> GetFeedsFromFolder(RssFolder parentFolder)
    {
        List<SyndicationItem> feed = new List<SyndicationItem>();

        foreach (RssFolder childFolder in parentFolder.Folders)
        {
            feed.AddRange(GetFeedsFromFolder(childFolder));
        }
        foreach (RssChannel channel in parentFolder.Channels)
        {
            feed.AddRange(channel.Feed().Items);
        }

        return feed;
    }

    static void ShowFeed(List<SyndicationItem> feed)
    {
        DateTime? releaseTime = ReleaseTime.NextReleaseTime();
        int index = 0;

        foreach (SyndicationItem item in feed)
        {
            if (item.PublishDate > releaseTime)
            {
                break;
            }

            Console.WriteLine(index++);
            Console.WriteLine($"Title: {item.Title.Text}");
            Console.WriteLine($"Published Date: {item.PublishDate}");
            Console.WriteLine($"Summary: {item.Summary.Text}");
            Console.WriteLine($"Link: {item.Id}");
            Console.WriteLine($"Author: {RssChannel.GetAuthors(item)}");
            Console.WriteLine();
        }
    }
}
