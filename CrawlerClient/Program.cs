using CrawlerLibrary;
using System;
using System.IO;

namespace CrawlerClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            string startLink = "put start link here";
            string saveLocation = "D:\\CrawledLinks";

            if (!Directory.Exists(saveLocation))
                Directory.CreateDirectory(saveLocation);

            var crawler = new WebSiteCrawler(startLink);

            var pagesCrawled = crawler.Crawl();

            foreach (var page in pagesCrawled)
            {
                string fileName = Path.Combine(saveLocation, $"{Uri.EscapeDataString(page.Url)}.html");

                File.WriteAllText(fileName, page.Content);
                Console.WriteLine($"Saved {page.Url} to {fileName}");
            }

            Console.WriteLine("Finished crawling");
        }
    }
}
