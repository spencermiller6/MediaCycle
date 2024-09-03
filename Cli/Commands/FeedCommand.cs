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

    private static IEnumerable<SyndicationItem> GetFeedFromSourceNames(string sourceName)
    {
        Cli.ChannelDictionary.TryGetValue(sourceName, out RssChannel? channel);

        if (channel is null)
        {
            throw new Exception($"'{sourceName}' is not a valid channel");
        }

        SyndicationFeed feed = channel.Feed();
        return feed.Items;
    }

    private static void Execute(List<string> sourceNames, bool verbose)
    {
        List<SyndicationItem> feed = new List<SyndicationItem>();

        if (sourceNames.Any())
        {
            foreach (string sourceName in sourceNames)
            {
                IEnumerable<SyndicationItem> items = GetFeedFromSourceNames(sourceName);
                feed.AddRange(items);
            }
        }
        else
        {
            foreach (RssChannel channel in Cli.ChannelDictionary.Values)
            {
                IEnumerable<SyndicationItem> items = channel.Feed().Items;
                feed.AddRange(items);
            }
        }

        DateTime? releaseTime = ReleaseTime.NextReleaseTime();
        List<SyndicationItem> filteredFeed = feed.Where(i => i.PublishDate < releaseTime).ToList();
        List<SyndicationItem> sortedFeed = filteredFeed.OrderByDescending(item => item.PublishDate).ToList();
        
        Cli.PresentFeed = sortedFeed;
        ShowFeed(sortedFeed, verbose);
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
