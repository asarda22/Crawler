
namespace CrawlerLibrary.Properties
{
    public class PageCrawled
    {
        public string Url { get; set; }
        public string Content { get; set; }

        public PageCrawled(string url, string content)
        {
            Url = url;
            Content = content;
        }
    }
}
