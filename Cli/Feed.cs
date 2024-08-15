using System;
using System.ServiceModel.Syndication;
using MediaCycle.Core;

namespace MediaCycle.Cli;

public class Feed : ICommand
{
    public string Name => "feed";

    public string HelpText => "Show the feed of all channels within the current scope";

    public List<object> Arguments => _arguments;

    public List<IOption> Options => _options;

    public void Execute()
    {
        ShowFeedsFromFolder(Cli.Pwd());
    }
        
    static void ShowFeedsFromFolder(RssFolder parentFolder)
    {
        foreach (RssFolder childFolder in parentFolder.Children)
        {

        }
        foreach (RssChannel channel in parentFolder.Children)
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

    private List<object> _arguments = new List<object>();
    private List<IOption> _options = new List<IOption>();
}
