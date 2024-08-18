using System.ServiceModel.Syndication;
using MediaCycle.Core;

namespace MediaCycle.Cli.Commands;

public class LsCommand : Command
{
    public override string Name => "ls";
    public override string HelpText => "Show the contents of the present working directory";
    public override int MinArguments => 0;
    public override int MaxArguments => 0;
    public override List<string> Arguments => _arguments;
    public override List<Option> Options => _options;

    private List<string> _arguments = new List<string>();
    private List<Option> _options = new List<Option>();

    public LsCommand(List<string> arguments, List<char> shortOptions, List<string> longOptions) : base(arguments, shortOptions, longOptions)
    {
    }

    public static void ShowFolder(RssFolder folder)
    {
        foreach (DirectoryItem child in folder.Folders)
        {
            Console.WriteLine($"{child.Name}");
        }

        foreach (DirectoryItem child in folder.Channels)
        {
            Console.WriteLine($"{child.Name}");
        }
    }

    public override void Execute()
    {
        ShowFolder(Cli.Pwd);
    }
}
