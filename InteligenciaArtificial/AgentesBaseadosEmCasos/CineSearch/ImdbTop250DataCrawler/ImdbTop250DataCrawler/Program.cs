using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abot.Crawler;
using Abot.Poco;

namespace ImdbTop250DataCrawler
{
    class Program
    {
        private const string Top250IMDB = "http://www.imdb.com/chart/top?ref_=nv_ch_250_4";

        static void Main(string[] args)
        {
            var webCrawler = new PoliteWebCrawler();
            webCrawler.PageCrawlStartingAsync += webCrawler_PageCrawlStartingAsync;
            webCrawler.PageCrawlCompletedAsync += webCrawler_PageCrawlCompletedAsync;
            webCrawler.PageCrawlDisallowedAsync += webCrawler_PageCrawlDisallowedAsync;
            webCrawler.PageLinksCrawlDisallowedAsync += webCrawler_PageLinksCrawlDisallowedAsync;

            webCrawler.Crawl(new Uri(Top250IMDB));
        }

        static void webCrawler_PageCrawlStartingAsync(object sender, PageCrawlStartingArgs e)
        {
            throw new NotImplementedException();
        }

        static void webCrawler_PageCrawlCompletedAsync(object sender, PageCrawlCompletedArgs e)
        {
            throw new NotImplementedException();
        }

        static void webCrawler_PageCrawlDisallowedAsync(object sender, PageCrawlDisallowedArgs e)
        {
            throw new NotImplementedException();
        }

        static void webCrawler_PageLinksCrawlDisallowedAsync(object sender, PageLinksCrawlDisallowedArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
