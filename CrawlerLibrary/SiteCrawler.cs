using CrawlerLibrary.Properties;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace CrawlerLibrary
{
    public class WebSiteCrawler
    {
        private HashSet<string> visitedUrls = new HashSet<string>();
        private string baseUrl;
        private HttpClient httpClient;

        public WebSiteCrawler(string baseUrl)
        {
            this.baseUrl = baseUrl;
            this.httpClient = new HttpClient();
        }

        public List<PageCrawled> Crawl()
        {
            var crawledPages = new List<PageCrawled>();
            CrawlPages(baseUrl, crawledPages);
            return crawledPages;
        }

        private void CrawlPages(string url,List<PageCrawled> crawledPages)
        {
            if (visitedUrls.Contains(url))
                return;

            visitedUrls.Add(url);

            try
            {
                var response = httpClient.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();

                var pageContent = response.Content.ReadAsStringAsync().Result;

                var crawledPage = new PageCrawled(url, pageContent);

                crawledPages.Add(crawledPage);

                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(pageContent);

                foreach (var hyperLink in htmlDocument.DocumentNode.SelectNodes("//a[@href]"))
                {
                    var href = hyperLink.GetAttributeValue("href", "");
                    var absoluteUri = new Uri(new Uri(url), href).AbsoluteUri;

                    if (Uri.IsWellFormedUriString(absoluteUri, UriKind.Absolute) &&
                        absoluteUri.StartsWith(baseUrl, StringComparison.OrdinalIgnoreCase) && !absoluteUri.StartsWith("mailto")
                        && !absoluteUri.Contains("@"))
                    {
                        CrawlPages(absoluteUri, crawledPages);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while crawling {url}: {ex.Message}");
            }

            Console.WriteLine("See crawled log here.");
        }
    }
}
