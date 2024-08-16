using System;

namespace MediaCycle.Cli.Commands;

public class PwdCommand : Command
{
    public override string Name => "pwd";
    public override string HelpText => "Show the present working directory";
    public override int MinArguments => 0;
    public override int MaxArguments => 0;
    public override List<string> Arguments => _arguments;
    public override List<Option> Options => _options;

    private List<string> _arguments = new List<string>();
    private List<Option> _options = new List<Option>();

    

    public PwdCommand(List<string> arguments, List<char> shortOptions, List<string> longOptions) : base(arguments, shortOptions, longOptions)
    {
    }

    public override void Execute()
    {
        string path = Cli.Pwd.ToPath();
        Console.WriteLine(path);
    }
}
