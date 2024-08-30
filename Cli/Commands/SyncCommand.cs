using System.CommandLine;
using MediaCycle.Sources;

namespace MediaCycle.Cli.Commands;

public static class SyncCommand
{
    public static Command Create()
    {
        var argument = new Argument<List<string>>("source-name")
        {
            Arity = ArgumentArity.ZeroOrMore
        };

        var command = new Command("sync", "Sync with your sources")
        {
            argument
        };

        command.SetHandler(Execute, argument);

        return command;
    }

    public static void Execute(List<string> sourceNames)
    {
        if (sourceNames.Any())
        {
            foreach (string sourceName in sourceNames)
            {
                ISource source = SourceFactory.BuildSource(sourceName);
                source.Sync();
            }
        }
        else
        {
            foreach (ISource source in SourceFactory.BuildAllSources())
            {
                source.Sync();
            }
        }
    }
}
