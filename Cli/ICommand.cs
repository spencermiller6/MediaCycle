using System;

namespace MediaCycle.Cli;

public interface ICommand
{
    string Name { get; }
    string HelpText { get; }
    List<object> Arguments { get; }
    List<IOption> Options { get; }
    void Execute();
}
