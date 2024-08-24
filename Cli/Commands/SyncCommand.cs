using MediaCycle.Sources;
using MediaCycle.Sync;

namespace MediaCycle.Cli.Commands;

public class SyncCommand : Command
{
    public SyncCommand(List<string> arguments, List<char> shortOptions, List<string> longOptions) : base(arguments, shortOptions, longOptions)
    {
    }

    public override string Name => "sync";
    public override string HelpText => "Sync with your sources";
    public override int MinArguments => 0;
    public override int MaxArguments => int.MaxValue;
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
        if (_arguments.Any())
        {
            foreach (string argument in _arguments)
            {
                ISource source = SourceFactory.BuildSource(argument);
                source.Sync();
            }
        }
        else
        {
            foreach (ISource source in SourceFactory.BuildAllSources())
            {
                source.Sync();
            }
        }
    }
}
