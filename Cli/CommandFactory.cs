using System;

namespace MediaCycle.Cli;

public class CommandFactory
{
    public static Command CreateCommand(string commandName, List<string> arguments, List<char> shortOptions, List<string> longOptions)
    {
        switch (commandName)
        {
            case "feed":
                return new FeedCommand(arguments, shortOptions, longOptions);
            default:
                throw new Exception($"\"{commandName}\" is not a recognized command");
        }
    }
}
