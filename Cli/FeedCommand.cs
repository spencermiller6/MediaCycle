using System;
using System.Collections.Immutable;
using System.ServiceModel.Syndication;
using MediaCycle.Core;

namespace MediaCycle.Cli;

public class FeedCommand : Command
{
    public override string Name => "feed";

    public override string HelpText => "Show the feed of all channels within the current scope";

    public override List<IArgument> Arguments => _arguments;

    public override List<Option> Options => _options;

    public override void Execute()
    {
        ShowFeedsFromFolder(Cli.Pwd());
    }
        
    static void ShowFeedsFromFolder(RssFolder parentFolder)
    {
        foreach (RssFolder childFolder in parentFolder.Folders)
        {

        }
        foreach (RssChannel channel in parentFolder.Channels)
        {
            
        }
    }

    static void ShowFeed(RssChannel channel)
    {
        DateTime? releaseTime = ReleaseTime.NextReleaseTime();

        Console.WriteLine($"Title: {channel.Feed().Title.Text}");
        Console.WriteLine($"Next Release Time: {releaseTime}");
        Console.WriteLine();
        
        Console.WriteLine($"{-1}\t<- {channel.Parent.Name}");
        Console.WriteLine();

        int index = 0;

        foreach (SyndicationItem item in channel.Feed().Items)
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

    public override void SetArguments(List<string> arguments)
    {
        if (arguments.Any())
        {
            throw new Exception($"Command \"{Name}\" does not accept any arguments");
        }
    }

    private List<IArgument> _arguments = new List<IArgument>();
    private List<Option> _options = new List<Option>();

    public FeedCommand(List<string> arguments, List<char> shortOptions, List<string> longOptions) : base(arguments, shortOptions, longOptions)
    {
    }
}
