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

            foreach (DirectoryItem child in folder.Children)
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

        static void ShowFeed(RssChannel channel)
        {
            if (channel.Feed is null)
            {
                channel.Feed = RssChannel.FetchRssFeed(channel.Url);
            }

            DateTime? releaseTime = ReleaseTime.NextReleaseTime();

            Console.WriteLine($"{-1}\t<- {channel.Parent.Name}");
            Console.WriteLine($"Title: {channel.Feed.Title.Text}");
            Console.WriteLine($"Next Release Time: {releaseTime}");

            int index = 0;

            foreach (SyndicationItem item in channel.Feed.Items)
            {
                if (item.PublishDate > releaseTime)
                {
                    break;
                }

                Console.WriteLine(index++);
                Console.WriteLine($"Title: {item.Title.Text}");
                Console.WriteLine($"Published Date: {item.PublishDate}");
                Console.WriteLine($"Summary: {item.Summary.Text}");
                Console.WriteLine($"Link: {item.Id}");
                Console.WriteLine($"Author: {RssChannel.GetAuthors(item)}");
                Console.WriteLine();
            }
        }
        
        static void ShowFeedsFromFolder(RssFolder folder)
        {
            throw new NotImplementedException();
        }
    }
}