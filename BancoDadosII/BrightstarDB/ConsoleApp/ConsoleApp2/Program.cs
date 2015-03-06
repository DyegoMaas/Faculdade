using System.Security.Cryptography;
using System.Text;
using BrightstarDB.Client;
using ConsoleApp2.Entidades;
using System;
using System.Diagnostics;
using System.Linq;

namespace ConsoleApp2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //InserirProdutosDadosManualmente();
            ExemploEntityFramework();
            Console.ReadLine();
        }

        private static void ExemploEntityFramework()
        {
            const string connectionString = "type=embedded;storesdirectory=.\\;storename=Filmes";
            var contexto = new MyEntityContext(connectionString);
            LimparFilmesDaBase(contexto);
            
            InserirFilmesEAtoresConhecidos(contexto);
            PopularABaseComFilmesEAtores(contexto);
            ImprimirNomesFilmes(contexto);

            //não é obrigatório criar um novo contexto
            contexto = new MyEntityContext(connectionString);

            BuscaPorIgualdade(contexto);
            BuscaContains(contexto);

            var atorEncontrado = contexto.Actors.First(ator => ator.Name.Contains("Ford"));
            Console.WriteLine(atorEncontrado.Id);
            Console.WriteLine(atorEncontrado.Name);
            Console.WriteLine(atorEncontrado.DateOfBirth);
            atorEncontrado.GetOlder(years: 10);
            contexto.SaveChanges();

            atorEncontrado = contexto.Actors.First(ator => ator.Name.Contains("Ford"));
            Console.WriteLine(atorEncontrado.Name);
            Console.WriteLine(atorEncontrado.DateOfBirth);

            var filmeEncontrado = contexto.Films.First(filme => filme.Name.Contains("Star"));
            Console.WriteLine(filmeEncontrado.Name);

            foreach (var filme in contexto.Films)
            {
                Console.WriteLine();
                Console.WriteLine("Atores em {0}", filme.Name);
                ImprimirAtoresDoFilme(filme);
            }
        }

        private static void ImprimirNomesFilmes(MyEntityContext contexto)
        {
            var watch = Stopwatch.StartNew();
            foreach (var filme in contexto.Films)
            {
                Console.WriteLine(filme.Name);
            }
            //ou ainda
            var nomesFilmes = from filme in contexto.Films
                select filme.Name;
            foreach (var nome in nomesFilmes)
            {
                Console.WriteLine(nome);
            }

            watch.Stop();
            ImprimirEmVerde(string.Format("TEMPO PARA IMPRIMIR FILMES: {0}", watch.Elapsed));
        }

        private static void LimparFilmesDaBase(MyEntityContext contexto)
        {
            foreach (var ator in contexto.Actors)
                contexto.DeleteObject(ator);

            foreach (var filme in contexto.Films)
                contexto.DeleteObject(filme);
        }

        private static void InserirFilmesEAtoresConhecidos(MyEntityContext contexto)
        {
            var bladeRunner = contexto.Films.Create();
            bladeRunner.Name = "Blade Runner";

            var starWars = contexto.Films.Create();
            starWars.Name = "StarWars";

            contexto.Films.Add(new Film
            {
                Name = "Jurassic Park"
            });

            var ford = contexto.Actors.Create();
            ford.Name = "Harrison Ford";
            ford.DateOfBirth = new DateTime(1942, 7, 13);
            ford.Films.Add(starWars);
            ford.Films.Add(bladeRunner);

            var hamill = contexto.Actors.Create();
            hamill.Name = "Mark Hamill";
            hamill.DateOfBirth = new DateTime(1951, 9, 25);
            hamill.Films.Add(starWars);

            contexto.SaveChanges();
        }

        private static void PopularABaseComFilmesEAtores(MyEntityContext contexto)
        {
            ImprimirEmVerde("INICIANDO CADASTRO DOS FILMES");

            var watch = Stopwatch.StartNew();
            for (int numeroFilmes = 1; numeroFilmes <= 50; numeroFilmes++)
            {
                var filmeAleatorioA = contexto.Films.Create();
                filmeAleatorioA.Name = "Filme aleatório A";

                var filmeAleatorioB = contexto.Films.Create();
                filmeAleatorioB.Name = "Filme aleatório B";

                for (int numeroAtores = 0; numeroAtores < 5; numeroAtores++)
                {
                    var atorAleatorio = contexto.Actors.Create();
                    atorAleatorio.Name = "Ator aleatório";
                    atorAleatorio.DateOfBirth = DateTime.Today;
                    atorAleatorio.Films.Add(filmeAleatorioA);
                    atorAleatorio.Films.Add(filmeAleatorioB);
                }
                contexto.SaveChanges();
                Console.WriteLine(numeroFilmes);
            }

            watch.Stop();
            ImprimirEmVerde(string.Format("TEMPO PARA CADASTRAR 100 FILMES e 500 ATORES: {0}", watch.Elapsed));
        }

        private static void ImprimirEmVerde(string texto)
        {
            var corOriginal = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine();
            Console.WriteLine(texto);

            Console.ForegroundColor = corOriginal;
        }

        private static void ImprimirAtoresDoFilme(IFilm filme)
        {
            foreach (var ator in filme.Actors)
            {
                Console.WriteLine("Ator: {0} ({1})", ator.Name, ator.Id);
            }
        }

        private static void BuscaPorIgualdade(MyEntityContext context)
        {
            var atorEncontrado = context.Actors.First(ator => ator.Name == "Mark Hamill");
            Console.WriteLine(atorEncontrado.Name);
        }

        private static void BuscaContains(MyEntityContext context)
        {
            var atorEncontrado = context.Actors.First(ator => ator.Name.Contains("Mark"));
            Console.WriteLine(atorEncontrado.Name);
        }

        private static void InserirProdutosDadosManualmente()
        {
            var data = new StringBuilder();
                data.AppendLine("<http://www.brightstardb.com/products/brightstar> <http://www.brightstardb.com/schemas/product/name> \"BrightstarDB\" .");
                data.AppendLine("<http://www.brightstardb.com/products/brightstar> <http://www.brightstardb.com/schemas/product/category> <http://www.brightstardb.com/categories/nosql> .");
                data.AppendLine("<http://www.brightstardb.com/products/brightstar> <http://www.brightstardb.com/schemas/product/category> <http://www.brightstardb.com/categories/.net> .");
                data.AppendLine("<http://www.brightstardb.com/products/brightstar> <http://www.brightstardb.com/schemas/product/category> <http://www.brightstardb.com/categories/rdf> .");
                data.AppendLine("<http://www.brightstardb.com/products/outrobanco> <http://www.brightstardb.com/schemas/product/category> <http://www.brightstardb.com/categories/sgbd> .");
                data.AppendLine("<http://www.brightstardb.com/products/outrobanco> <http://www.brightstardb.com/schemas/product/name> \"Outro Banco :)\" .");

            var client = BrightstarService.GetClient("type=embedded;storesdirectory=.\\");
            
            const string storeName = "produtos";
            if(!client.DoesStoreExist(storeName))
                client.CreateStore(storeName);
            client.ListStores().ToList().ForEach(Console.WriteLine);

            client.ExecuteTransaction(storeName, null, null, data.ToString());
        }
    }
}
