using System.CommandLine;
using MediaCycle.Cli.Commands;

public class Program
{
    static async Task<int> Main(string[] args)
    {
        var rootCommand = new RootCommand
        {
            Name = "mediacycle",
            Description = "A CLI RSS reader designed to control the release of content"
        };

        rootCommand.AddCommand(ConnectCommand.Create());
        rootCommand.AddCommand(FeedCommand.Create());
        rootCommand.AddCommand(OpenCommand.Create());
        rootCommand.AddCommand(SyncCommand.Create());

        return await rootCommand.InvokeAsync(args);
    }
}
