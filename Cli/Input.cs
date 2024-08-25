using System.Text;

namespace MediaCycle.Cli;

public static class Input
{
    public static string ReadLineWithTabCompletion()
    {
        List<string> itemNames = Cli.Pwd.Folders.Select(i => i.Name).ToList();
        itemNames.AddRange(Cli.Pwd.Channels.Select(i => i.Name).ToList());

        StringBuilder input = new StringBuilder();
        int cursorPos = 0;

        while (true)
        {
            var key = Console.ReadKey(intercept: true);

            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                return input.ToString();
            }
            else if (key.Key == ConsoleKey.Backspace)
            {
                if (cursorPos > 0)
                {
                    input.Remove(cursorPos - 1, 1);
                    cursorPos--;
                    Console.Write("\b \b");
                }
            }
            else if (key.Key == ConsoleKey.Tab)
            {
                // Get the current word based on the cursor position
                string[] words = input.ToString().Split(' ');
                int currentWordIndex = GetCurrentWordIndex(input.ToString(), cursorPos);
                string currentWord = words[currentWordIndex];

                // Find matches for the current word
                var matches = itemNames.Where(o => o.StartsWith(currentWord, StringComparison.Ordinal)).ToList();

                if (matches.Count == 1)
                {
                    string autofill = matches[0].Substring(currentWord.Length);
                    input.Insert(cursorPos, autofill);

                    Console.Write(autofill);
                    cursorPos += autofill.Length;
                }
                else if (matches.Count > 1)
                {
                    string autofill = GetCommonPrefix(matches).Substring(currentWord.Length);
                    input.Insert(cursorPos, autofill);

                    Console.Write(autofill);
                    cursorPos += autofill.Length;
                }
            }
            else
            {
                input.Insert(cursorPos, key.KeyChar);
                cursorPos++;
                Console.Write(key.KeyChar);
            }
        }
    }

    static int GetCurrentWordIndex(string input, int cursorPos)
    {
        string[] words = input.Substring(0, cursorPos).Split(' ');
        return words.Length - 1;
    }

    static string GetCommonPrefix(List<string> matches)
    {
        if (!matches.Any()) return string.Empty;

        string prefix = matches[0];
        for (int i = 1; i < matches.Count; i++)
        {
            prefix = GetCommonPrefix(prefix, matches[i]);
            if (prefix.Length == 0) break;
        }
        return prefix;
    }

    static string GetCommonPrefix(string a, string b)
    {
        int length = Math.Min(a.Length, b.Length);
        for (int i = 0; i < length; i++)
        {
            if (a[i] != b[i])
            {
                return a.Substring(0, i);
            }
        }
        return a.Substring(0, length);
    }
}
