using System.Diagnostics;
using System.ServiceModel.Syndication;

namespace MediaCycle.Cli.Commands;

public class OpenCommand : Command
{
    public override string Name => "open";
    public override string HelpText => "Open an item in the browser";
    public override int MinArguments => 1;
    public override int MaxArguments => 1;
    public override List<string> Arguments => _arguments;
    public override List<Option> Options => _options;

    private List<string> _arguments = new List<string>();
    private List<Option> _options = new List<Option>();

    public OpenCommand(List<string> arguments, List<char> shortOptions, List<string> longOptions) : base(arguments, shortOptions, longOptions)
    {
    }

    public override void SetArguments(List<string> arguments)
    {
        base.SetArguments(arguments);
        _arguments = arguments;
    }

    public SyndicationItem ParseItemByIndex(int index)
    {
        if (Cli.PresentFeed is null)
        {
            throw new Exception("You must load a feed before opening an item");
        }
        if (index < 0 || index > Cli.PresentFeed.Count)
        {
            throw new Exception($"'{Arguments[0]}' is not a valid index");
        }

        return Cli.PresentFeed[index];
    }

    public void OpenUrlInBrowser(string url)
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
        catch (Exception)
        {
            Console.WriteLine($"Failed to open URL");
        }
    }

    public override void Execute()
    {
        if (!int.TryParse(Arguments[0], out int index))
        {
            throw new Exception($"'{Arguments[0]}' is not a valid index");
        }

        string url = ParseItemByIndex(index).Id;
        OpenUrlInBrowser(url);
    }
}
