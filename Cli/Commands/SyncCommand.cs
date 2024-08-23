using MediaCycle.Sync;

namespace MediaCycle.Cli.Commands;

public class SyncCommand : Command
{
    public SyncCommand(List<string> arguments, List<char> shortOptions, List<string> longOptions) : base(arguments, shortOptions, longOptions)
    {
    }

    public override string Name => "sync";
    public override string HelpText => "Syncs your feeds from other platforms";
    public override int MinArguments => 0;
    public override int MaxArguments => 0;
    public override List<string> Arguments => _arguments;
    public override List<Option> Options => _options;

    private List<string> _arguments = new List<string>();
    private List<Option> _options = new List<Option>();

    public override void Execute()
    {
        Youtube.Execute();
    }
}
