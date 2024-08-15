using System;
using MediaCycle.Core;

namespace MediaCycle.Cli;

public class CdCommand : Command
{
    public override string Name => "cd";
    public override string HelpText => "Change the directory";
    public override int MinArguments => 1;
    public override int MaxArguments => 1;
    public override List<string> Arguments => _arguments;
    public override List<Option> Options => _options;

    private List<string> _arguments = new List<string>();
    private List<Option> _options = new List<Option>();

    public CdCommand(List<string> arguments, List<char> shortOptions, List<string> longOptions) : base(arguments, shortOptions, longOptions)
    {
    }

    public RssFolder ParsePath()
    {
        RssFolder pwd = Cli.Pwd;
        string path = Arguments[0];
        string[] directories = path.Split(['/']);

        for (int i = 0; i < directories.Length; i ++)
        {
            switch (directories[i])
            {
                case "~":
                    pwd = RssFolder.Root();
                    break;
                case ".":
                    break;
                case "..":
                    if (pwd.Parent is not null)
                    {
                        pwd = pwd.Parent;
                    }
                    
                    break;
                default:
                    RssFolder? folder = pwd.Folders.FirstOrDefault(f => f.Name == directories[i]);

                    if (folder is null)
                    {
                        throw new Exception($"{Name}: {path}: No such directory");
                    }
                    else
                    {
                        pwd = folder;
                    }

                    break;
            }
        }

        return pwd;
    }

    public override void SetArguments(List<string> arguments)
    {
        base.SetArguments(arguments);
        _arguments = arguments;
    }

    public override void Execute()
    {
        Cli.Pwd = ParsePath();
    }
}
