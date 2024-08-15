using System;

namespace MediaCycle.Cli;

public class Option
{
    public char? ShortName;
    public string? LongName;
    public string? HelpText;
    public bool IsSet = false;

    public Option()
    {

    }
}
