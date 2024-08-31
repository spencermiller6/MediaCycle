using System.CommandLine;
using System.ServiceModel.Syndication;
using MediaCycle.Core;

namespace MediaCycle.Cli.Commands;

public static class FeedCommand
{
    public static Command Create()
    {
        var argument = new Argument<List<string>>("source-name")
        {
            Arity = ArgumentArity.ZeroOrMore
        };

        argument.AddCompletions((completionContext) =>
        {
            return Cli.Completions;
        });

        var verboseOption = new Option<bool>(
            aliases: new string[] { "--verbose", "-v" },
            description: "Enable verbose output",
            getDefaultValue: () => false
        );

        var command = new Command("feed", "Show the feed of all channels within the current scope")
        {
            argument,
            verboseOption
        };

        command.SetHandler(Execute, argument, verboseOption);

        return command;
    }

    private static void Execute(List<string> sourceNames, bool verbose)
    {
        List<SyndicationItem> feed;

        if (sourceNames.Any())
        {
            feed = new List<SyndicationItem>();
            foreach (string sourceName in sourceNames)
            {
                RssChannel channel = GetChannelFromString(sourceName);
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
        
        Cli.PresentFeed = sortedFeed;
        ShowFeed(sortedFeed, verbose);
    }

    private static RssChannel GetChannelFromString(string channelName)
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
        
    private static List<SyndicationItem> GetFeedsFromFolder(RssFolder parentFolder)
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

    private static void ShowFeed(List<SyndicationItem> feed, bool verbose)
    {
        int index = 0;

        foreach (SyndicationItem item in feed)
        {
            Console.WriteLine($"{index++}\t{item.Title.Text}");

            if (verbose)
            {
                Console.WriteLine($"\tPosted by {item.Authors[0].Name} on {item.PublishDate}");

                if (item.Summary is not null)
                {
                    Console.WriteLine($"\n\t{item.Summary.Text}\n");
                }

                Console.WriteLine();
            }
        }
    }
}
