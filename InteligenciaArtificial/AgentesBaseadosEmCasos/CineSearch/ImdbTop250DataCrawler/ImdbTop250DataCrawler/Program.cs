using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace ImdbTop250DataCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            var linksTop250 = ObterLinksTop250().ToArray();

            using (var arquivoCsv = new FileStream("top250IMDB.csv", FileMode.Create))
            {
                var bytes = ToByteArray("Posicao;Nota;Nome;Gênero;Duracao;Faixa Etária;Ano" + Environment.NewLine);
                arquivoCsv.Write(bytes, 0, bytes.Length);

                for (int i = 0; i < linksTop250.Length; i++)
                {
                    Console.WriteLine("Extraindo informacoes do filme " + (i+1));

                    var filmeIMDB = ExtrairInformacoesPagina(linksTop250[i]);
                    var linhaCsv = MontarLinhaCsv(filmeIMDB, i+1);

                    bytes = Encoding.UTF8.GetBytes(linhaCsv + Environment.NewLine);
                    arquivoCsv.Write(bytes, 0, bytes.Length);
                }
            }

            Console.WriteLine("Good job!");
            Console.Read();
        }

        private static byte[] ToByteArray(string texto)
        {
            return Encoding.UTF8.GetBytes(texto);
        }

        private static string MontarLinhaCsv(IFilmeIMDB filme, int posicao)
        {
            return string.Format("{0};{1};{2};{3};{4};{5};{6}", 
                posicao,
                filme.Nota,
                filme.Nome,
                string.Join(",", filme.Generos),
                filme.DuracaoMinutos,
                filme.FaixaEtaria,
                filme.Ano);
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

        private static IFilmeIMDB ExtrairInformacoesPagina(string link)
        {
            using (var client = new WebClient())
            {
                var htmlPaginaFilme = Encoding.UTF8.GetString(client.DownloadData(link));
                return new ExtratorInformacoesFilmeIMDB(htmlPaginaFilme);
            }
        }
    }

    internal interface IFilmeIMDB
    {
        string Nome { get; }
        string Nota { get; }
        int Ano { get; }
        string DuracaoMinutos { get; }
        int FaixaEtaria { get; }
        IList<string> Generos { get; }
    }

    class ExtratorInformacoesFilmeIMDB : IFilmeIMDB
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
            get
            {
                var ano = Extrair("<a href=\"\\/year\\/(\\d{4})\\/");
                if (ano.Length == 1)
                    ano = "0" + ano;

                if (ano.Length == 2)
                    ano = "20" + ano;
                
                return int.Parse(ano);
            }
        }

        public string DuracaoMinutos
        {
            get { return Extrair("(\\d+) min"); }
        }

        public int FaixaEtaria
        {
            get
            {
                var extrair = Extrair("itemprop=\"contentRating\" content=\"(\\d+)\"");
                if (extrair == string.Empty)
                    return 0;//tratar
                return int.Parse(extrair);
            }
        }

        public IList<string> Generos
        {
            get
            {
                var generos = new List<string>();

                var matches = Regex.Matches(htmlPagina, "itemprop=\"genre\">(.*)<\\/span>");
                foreach (var match in matches)
                {
                    var genero = ((Match)match).Groups[1].Value;

                    var indiceMenor = genero.IndexOf("<");
                    if (indiceMenor > -1)
                        genero = genero.Substring(0, indiceMenor);

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
