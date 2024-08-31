using System.CommandLine;
using MediaCycle.Cli;
using MediaCycle.Cli.Commands;

public class Program
{
    static async Task<int> Main(string[] args)
    {
        var rootCommand = new RootCommand("Sample app for System.CommandLine");

        rootCommand.AddCommand(CdCommand.Create());
        rootCommand.AddCommand(ConnectCommand.Create());
        rootCommand.AddCommand(FeedCommand.Create());
        rootCommand.AddCommand(LsCommand.Create());
        rootCommand.AddCommand(OpenCommand.Create());
        rootCommand.AddCommand(PwdCommand.Create());
        rootCommand.AddCommand(SyncCommand.Create());

        return await rootCommand.InvokeAsync(args);
    }
}
