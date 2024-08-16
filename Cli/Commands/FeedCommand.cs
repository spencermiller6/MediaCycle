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
    public override int MaxArguments => 1;
    public override List<string> Arguments => _arguments;
    public override List<Option> Options => _options;

    private List<string> _arguments = new List<string>();
    private List<Option> _options = new List<Option>();

    public FeedCommand(List<string> arguments, List<char> shortOptions, List<string> longOptions) : base(arguments, shortOptions, longOptions)
    {
    }

    public override void SetArguments(List<string> arguments)
    {
        base.SetArguments(arguments);
        _arguments = arguments;
    }

    public override void Execute()
    {
        List<SyndicationItem> feed;

        if (Arguments.Any())
        {
            feed = new List<SyndicationItem>();
            foreach (string argument in Arguments)
            {
                RssChannel channel = GetChannelFromString(argument);
                feed.AddRange(channel.Feed().Items);
            }
        }
        else
        {
            feed = GetFeedsFromFolder(Cli.Pwd);
        }

        DateTime? releaseTime = ReleaseTime.NextReleaseTime();
        List<SyndicationItem> filteredFeed = feed.Where(i => i.PublishDate < releaseTime).ToList();
        List<SyndicationItem> sortedFeed = filteredFeed.OrderByDescending(item => item.PublishDate).ToList();
        
        ShowFeed(sortedFeed);
    }

    static RssChannel GetChannelFromString(string channelName)
    {
        RssChannel? channel = Cli.Pwd.Channels.FirstOrDefault(c => c.Name == channelName);

        if(channel is null)
        {
            throw new Exception($"{channelName}: No such directory");
        }
        else
        {
            return channel;
        }
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
        int index = 0;

        foreach (SyndicationItem item in feed)
        {
            Console.WriteLine(index++);
            Console.WriteLine($"Title: {item.Title.Text}");
            Console.WriteLine($"Published Date: {item.PublishDate}");
            Console.WriteLine($"Summary: {item.Summary?.Text ?? string.Empty}");
            Console.WriteLine($"Link: {item.Id}");
            Console.WriteLine($"Author: {RssChannel.GetAuthors(item)}");
            Console.WriteLine();
        }
    }
}
