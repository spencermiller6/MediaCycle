using System.CommandLine;
using MediaCycle.Core;

namespace MediaCycle.Cli.Commands;

public static class LsCommand
{
    public static Command Create()
    {
        var command = new Command("ls", "Show the contents of the present working directory");

        command.SetHandler(Execute);

        return command;
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

    public static void Execute()
    {
        ShowFolder(Cli.Pwd);
    }
}
