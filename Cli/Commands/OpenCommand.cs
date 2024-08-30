using System.CommandLine;
using System.Diagnostics;
using System.ServiceModel.Syndication;

namespace MediaCycle.Cli.Commands;

public static class OpenCommand
{
    public static Command Create()
    {
        var argument = new Argument<int>("item-index")
        {
            Arity = ArgumentArity.ExactlyOne
        };

        var command = new Command("open", "Open an item in the browser")
        {
            argument
        };

        command.SetHandler(Execute, argument);

        return command;
    }

    public static SyndicationItem ParseItemByIndex(int index)
    {
        if (Cli.PresentFeed is null)
        {
            throw new Exception("You must load a feed before opening an item");
        }
        if (index < 0 || index > Cli.PresentFeed.Count)
        {
            throw new Exception($"'{index}' is not a valid index");
        }

        return Cli.PresentFeed[index];
    }

    public static void OpenUrlInBrowser(string url)
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
        catch (Exception)
        {
            Console.WriteLine($"Failed to open URL");
        }
    }

    public static void Execute(int itemIndex)
    {
        string url = ParseItemByIndex(itemIndex).Links[0].Uri.ToString();
        OpenUrlInBrowser(url);
    }
}
