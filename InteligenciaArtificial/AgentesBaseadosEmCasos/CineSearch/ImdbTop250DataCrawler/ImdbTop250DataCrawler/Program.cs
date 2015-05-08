using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abot.Crawler;
using Abot.Poco;

namespace ImdbTop250DataCrawler
{
    class Program
    {
        private const string Top250IMDB = "http://www.imdb.com/chart/top?ref_=nv_ch_250_4";
        //private static IEnumerable<string> Top250IMDB
        //{
        //    get
        //    {
        //        for (int i = 1; i <= 250; i++)
        //        {
        //            yield return "http://www.imdb.com/title/tt0111495/?ref_=chttp_tt_" + i;
        //        }
        //    }
        //} 

        static void Main(string[] args)
        {
            //var webRequest = HttpWebRequest.Create("http://www.imdb.com/chart/top?ref_=nv_ch_250_4");
            //var webResponse = webRequest.GetResponse();

            var linksTop250 = ObterLinksTop250();

            //var webCrawler = new PoliteWebCrawler();
            //webCrawler.PageCrawlStartingAsync += webCrawler_PageCrawlStartingAsync;
            //webCrawler.PageCrawlCompletedAsync += webCrawler_PageCrawlCompletedAsync;
            //webCrawler.PageCrawlDisallowedAsync += webCrawler_PageCrawlDisallowedAsync;
            //webCrawler.PageLinksCrawlDisallowedAsync += webCrawler_PageLinksCrawlDisallowedAsync;

            foreach (var link in linksTop250)
            {
                ExtrairInformacoesPagina(link);
                //webCrawler.Crawl(new Uri(link));
            }
        }

        private static IEnumerable<string> ObterLinksTop250()
        {
            var links = new List<string>();
            using (var client = new WebClient())
            {
                var paginaTop250 = client.DownloadString("http://www.imdb.com/chart/top?ref_=nv_ch_250_4");

                var matches = Regex.Matches(paginaTop250, @"/title\/tt\d{7}\/\?ref_=chttp_tt_\d{1,3}");
                foreach (var match in matches)
                {
                    var link = "http://www.imdb.com" + match;
                    if (!links.Contains(link))
                    {
                        links.Add(link);
                    }
                }
            }
            var obterLinksTop250 = links.Distinct();
            return obterLinksTop250;
        }

        private static void ExtrairInformacoesPagina(string link)
        {
            using (var client = new WebClient())
            {
                var htmlPaginaFilme = client.DownloadString(link);
                var extrator = new ExtratorInformacoesFilmeIMDB(htmlPaginaFilme);
                var nota = extrator.Nota;
                var ano = extrator.Ano;
                var nome = extrator.Nome;
                var generos = extrator.Generos;
            }
        }

        

        static void webCrawler_PageCrawlStartingAsync(object sender, PageCrawlStartingArgs e)
        {
        }

        static void webCrawler_PageCrawlCompletedAsync(object sender, PageCrawlCompletedArgs e)
        {
            //var links = e.CrawledPage.ParsedLinks.Select(link =>
            //{
            //    return true;
            //});
            

            throw new NotImplementedException();
        }

        static void webCrawler_PageCrawlDisallowedAsync(object sender, PageCrawlDisallowedArgs e)
        {
        }

        static void webCrawler_PageLinksCrawlDisallowedAsync(object sender, PageLinksCrawlDisallowedArgs e)
        {
        }
    }

    class ExtratorInformacoesFilmeIMDB
    {
        private readonly string htmlPagina;

        public ExtratorInformacoesFilmeIMDB(string htmlPagina)
        {
            this.htmlPagina = htmlPagina;
        }

        public string Nome
        {
            get { return Extrair("<span class=\"itemprop\" itemprop=\"name\">(.*)<\\/span>"); }
        }

        public string Nota
        {
            get { return Extrair("star-box-giga-star\">(.*)<"); }
        }

        public int Ano
        {
            get { return int.Parse(Extrair("<a href=\"\\/year\\/(\\d{4})\\/")); }
        }

        public IList<string> Generos
        {
            get
            {
                var generos = new List<string>();
                //var match = Regex.Match(htmlPagina, "itemprop=\"genre\">(.*)<\\/span>");
                //while (match != null)
                //{
                //    var genero = match.Value;
                //    if (!generos.Contains(genero))
                //    {
                //        generos.Add(genero);
                //    }

                //    match = match.NextMatch();
                //}
                var matches = Regex.Matches(htmlPagina, "itemprop=\"genre\">(.*)<\\/span>");
                foreach (var match in matches)
                {
                    var genero = ((Match)match).Groups[1].Value;
                    if (!generos.Contains(genero))
                    {
                        generos.Add(genero);
                    }
                }
                return generos;
            }
        }

        private string Extrair(string pattern)
        {
            var match = Regex.Match(htmlPagina, pattern);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            return string.Empty;
        }
    }
}
