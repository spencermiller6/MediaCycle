using System.ServiceModel.Syndication;
using MediaCycle.Core;

namespace MediaCycle.Cli.Navigation
{
    public static class Navigation
    {
        public static void ShowFolder(RssFolder folder)
        {
            int index = 0;

            if (folder.Parent is not null)
            {
                Console.WriteLine($"{-1}\t<- {folder.Parent.Name}");
            }

            foreach (DirectoryItem child in folder.Folders)
            {
                Console.WriteLine($"{index++}\t{child.Name}");
            }

            foreach (DirectoryItem child in folder.Channels)
            {
                Console.WriteLine($"{index++}\t{child.Name}");
            }
            
            try
            {
                int selection = int.Parse(Console.ReadLine());

                if (selection == -1)
                {
                    ShowFolder(folder.Parent);
                }
                if (selection >= 0 && selection < folder.Children.Count)
                {
                    if (folder.Children[selection] is RssFolder directory)
                    {
                        ShowFolder(directory);
                    }
                    else if (folder.Children[selection] is RssChannel channel)
                    {
                        ShowFeed(channel);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("You must provide a valid selection.");
            }
        }
    }
}