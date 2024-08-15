using System;

namespace MediaCycle.Cli;

public interface IArgument
{
    public object Value { get; }
}
