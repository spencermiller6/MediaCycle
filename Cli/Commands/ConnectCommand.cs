using System.CommandLine;
using MediaCycle.Sources;

namespace MediaCycle.Cli.Commands;

public static class ConnectCommand
{
    public static Command Create()
    {
        var argument = new Argument<string>("source-name")
        {
            Arity = ArgumentArity.ExactlyOne
        };

        var command = new Command("connect", "Connect to an RSS source")
        {
            argument
        };

        command.SetHandler(Execute, argument);

        return command;
    }

    private static void Execute(string sourceName)
    {
        ISource source = SourceFactory.BuildSource(sourceName);
        source.Connect();
    }
}
