using System;

namespace MediaCycle.Cli;

public interface IOption
{
    char ShortName { get; }
    string LongName { get; }
    string HelpText { get; }
    bool IsSet { get; }
}
