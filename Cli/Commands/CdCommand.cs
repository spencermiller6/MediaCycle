using System.CommandLine;
using MediaCycle.Core;

namespace MediaCycle.Cli.Commands;

public static class CdCommand
{
    public static Command Create()
    {
        var argument = new Argument<string>("path")
        {
            Arity = ArgumentArity.ExactlyOne
        };

        var command = new Command("cd", "Change the directory")
        {
            argument
        };

        command.SetHandler(Execute, argument);

        return command;
    }

    private static void Execute(string path)
    {
        Cli.Pwd = ParsePath(path);
    }

    public static RssFolder ParsePath(string path)
    {
        RssFolder pwd = Cli.Pwd;
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
                        throw new Exception($"{path}: No such directory");
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
}
