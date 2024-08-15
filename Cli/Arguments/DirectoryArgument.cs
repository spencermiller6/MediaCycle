using System;
using MediaCycle.Core;

namespace MediaCycle.Cli;

public class DirectoryArgument : IArgument
{
    public object Value => _value;

    public DirectoryArgument()
    {
        
    }

    private DirectoryItem _value;
}
