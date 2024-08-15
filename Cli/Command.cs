using System;

namespace MediaCycle.Cli;

public abstract class Command
{
    public abstract string Name { get; }
    public abstract string HelpText { get; }
    public abstract List<IArgument> Arguments { get; }
    public abstract List<Option> Options { get; }

    public Command(List<string> arguments, List<char> shortOptions, List<string> longOptions)
    {
        SetArguments(arguments);
        SetOptions(shortOptions, longOptions);
    }

    public abstract void SetArguments(List<string> arguments);

    public void SetOptions(List<char> shortOptions, List<string> longOptions)
    {
        foreach (char shortOption in shortOptions)
        {
            Option? option = Options.FirstOrDefault(option => option.ShortName == shortOption);
            if (option != null)
            {
                option.IsSet = true;
            }
            else
            {
                throw new Exception($"Command \"{Name}\" does not accept any arguments");
            }
        }
    }

    public abstract void Execute();
}
