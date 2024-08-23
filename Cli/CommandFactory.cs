using MediaCycle.Cli.Commands;

namespace MediaCycle.Cli;

public class CommandFactory
{
    public static Command CreateCommand(string commandName, List<string> arguments, List<char> shortOptions, List<string> longOptions)
    {
        switch (commandName)
        {
            case "cd":
                return new CdCommand(arguments, shortOptions, longOptions);
            case "feed":
                return new FeedCommand(arguments, shortOptions, longOptions);
            case "ls":
                return new LsCommand(arguments, shortOptions, longOptions);
            case "open":
                return new OpenCommand(arguments, shortOptions, longOptions);
            case "pwd":
                return new PwdCommand(arguments, shortOptions, longOptions);
            case "sync":
                return new SyncCommand(arguments, shortOptions, longOptions);
            default:
                throw new Exception($"\"{commandName}\" is not a recognized command");
        }
    }
}
