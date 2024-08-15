using System;

namespace MediaCycle.Cli;

public class CommandFactory
{
    public static ICommand CreateCommand(string commandName)
    {
        ICommand command;
        switch (commandName)
        {
            case "feed":
                command = new Feed();
                break;
            default:
                throw new Exception($"\"{commandName}\" not a recognized command");
        }

        command.SetArguments();
        command.SetOptions();
    }
}
