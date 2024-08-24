using System;
using MediaCycle.Sources;
using MediaCycle.Sync;

namespace MediaCycle.Cli.Commands;

public class ConnectCommand : Command
{
    public ConnectCommand(List<string> arguments, List<char> shortOptions, List<string> longOptions) : base(arguments, shortOptions, longOptions)
    {
    }

    public override string Name => "connect";
    public override string HelpText => "Connect to an RSS source";
    public override int MinArguments => 1;
    public override int MaxArguments => 1;
    public override List<string> Arguments => _arguments;
    public override List<Option> Options => _options;

    private List<string> _arguments = new List<string>();
    private List<Option> _options = new List<Option>();

    public override void SetArguments(List<string> arguments)
    {
        base.SetArguments(arguments);
        _arguments = arguments;
    }

    public override void Execute()
    {
        ISource source = SourceFactory.BuildSource(_arguments[0]);
        source.Connect();
    }
}
