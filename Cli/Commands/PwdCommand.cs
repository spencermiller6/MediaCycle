using System.CommandLine;

namespace MediaCycle.Cli.Commands;

public static class PwdCommand
{
    public static Command Create()
    {
        var command = new Command("pwd", "Show the present working directory");

        command.SetHandler(Execute);

        return command;
    }

    public static void Execute()
    {
        string path = Cli.Pwd.ToPath();
        Console.WriteLine(path);
    }
}
