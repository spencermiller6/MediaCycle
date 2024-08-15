using MediaCycle.Core;

namespace MediaCycle.Cli;

public static class Cli
{
    public static void Start()
    {
        while(true)
        {
            string? input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
            {
                ParseInput(input);
            }
        }
    }

    public static void ParseInput(string input)
    {
        string? name;
        List<char> shortOptions = new List<char>();
        List<string> longOptions = new List<string>();
        List<string> arguments = new List<string>();

        var words = input.Split([' '], StringSplitOptions.RemoveEmptyEntries);
        name = words[0];

        for (int i = 1; i < words.Length; i++)
        {
            string word = words[i];

            if (word.StartsWith("--"))
            {
                longOptions.Add(word);
            }
            else if (word.StartsWith('-'))
            {
                foreach (char option in word.Substring(1))
                {
                    shortOptions.Add(option);
                }
            }
            else
            {
                arguments.Add(word);
            }
        }

        CommandFactory.CreateCommand(name, arguments, shortOptions, longOptions);
    }

    public static RssFolder Pwd()
    {
        if (_pwd is null)
        {
            _pwd = RssFolder.Root();
        }

        return _pwd;
    }

    private static RssFolder? _pwd;
}
