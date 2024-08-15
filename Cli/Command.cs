using System;

namespace MediaCycle.Cli;

public abstract class Command
{
    public abstract string Name { get; }
    public abstract string HelpText { get; }
    public abstract int MinArguments { get; }
    public abstract int MaxArguments { get; }
    public abstract List<string> Arguments { get; }
    public abstract List<Option> Options { get; }

    public Command(List<string> arguments, List<char> shortOptions, List<string> longOptions)
    {
        SetArguments(arguments);
        SetOptions(shortOptions, longOptions);
    }

    public virtual void SetArguments(List<string> arguments)
    {
        if (arguments.Count() < MinArguments)
        {
            throw new Exception($"Not enough arguments");
        }
        if (arguments.Count() > MaxArguments)
        {
            throw new Exception($"Too many arguments");
        }
    }

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
                throw new Exception($"Unknown switch '{shortOption}'");
            }
        }

        foreach (string longOption in longOptions)
        {
            Option? option = Options.FirstOrDefault(option => option.LongName == longOption);
            if (option != null)
            {
                option.IsSet = true;
            }
            else
            {
                throw new Exception($"Unknown option '{longOption}'");
            }
        }
    }

    public abstract void Execute();
}
