using ConsoleApp2.Entidades;
using System;
using System.Diagnostics;
using System.Linq;

namespace ConsoleApp2
{
    class Program
    {
        const string ConnectionString = "type=embedded;storesdirectory=.\\;storename=Filmes";

        static void Main(string[] args)
        {
            var contexto = new MyEntityContext(ConnectionString);
            
            LimparABase(contexto);
            InserirFilmesEAtoresConhecidos(contexto);
            PopularABase(contexto);
            ImprimirNomesFilmes(contexto);

            //não é obrigatório criar um novo contexto
            contexto = new MyEntityContext(ConnectionString);

            TesteDesempenhoQueryIgualdade(contexto);
            TesteDesempenhoQueryContains(contexto);

            var atorEncontrado = contexto.Actors.First(ator => ator.Name.Contains("Ford"));
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

            Console.ReadLine();
        }

        private static void ImprimirNomesFilmes(MyEntityContext contexto)
        {
            var watch = Stopwatch.StartNew();
            foreach (var filme in contexto.Films)
            {
                Console.WriteLine(filme.Name);
            }
            watch.Stop();
            ImprimirEmVerde(string.Format("TEMPO PARA IMPRIMIR FILMES: {0}", watch.Elapsed));
        }

        private static void LimparABase(MyEntityContext contexto)
        {
            foreach(var ator in contexto.Actors)
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

        private static void PopularABase(MyEntityContext contexto)
        {
            ImprimirEmVerde("INICIANDO CADASTRO DOS FILMES");

            var watch = Stopwatch.StartNew();
            for (int numeroFilmes = 1; numeroFilmes <= 500; numeroFilmes++)
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
            ImprimirEmVerde(string.Format("TEMPO PARA CADASTRAR 1000 FILMES: {0}", watch.Elapsed));
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

        private static void TesteDesempenhoQueryIgualdade(MyEntityContext context)
        {
            ImprimirEmVerde("BUSCANDO 30 VEZES O ATOR Mark Hamill");

            for (int i = 0; i < 30; i++)
            {
                var watch = Stopwatch.StartNew();
                var atorEncontrado = context.Actors.First(ator => ator.Name == "Mark Hamill");
                watch.Stop();

                Console.WriteLine(watch.Elapsed);
            }

            //var query = "SELECT ?category WHERE { " +
            //"<http://www.brightstardb.com/products/brightstar> <http://brightstardb.com/namespaces/default/name> ?category " +
            //"}";
        }

        private static void TesteDesempenhoQueryContains(MyEntityContext context)
        {
            ImprimirEmVerde("BUSCANDO 30 VEZES O ATOR cujo nome contém com Mark");

            for (int i = 0; i < 30; i++)
            {
                var watch = Stopwatch.StartNew();
                var atorEncontrado = context.Actors.First(ator => ator.Name.Contains("Mark"));
                watch.Stop();

                Console.WriteLine(watch.Elapsed);
            }

            //var query = "SELECT ?category WHERE { " +
            //"<http://www.brightstardb.com/products/brightstar> <http://brightstardb.com/namespaces/default/name> ?category " +
            //"}";
        }
























        //private static void TesteDesempenhoQuerySPARQLContains(MyEntityContext context)
        //{
        //    ImprimirEmVerde("BUSCANDO 30 VEZES O ATOR cujo nome contém com Mark");

        //    var client = BrightstarService.GetClient("type=embedded;storesdirectory=.\\");
        //    for (int i = 0; i < 30; i++)
        //    {
        //        var watch = Stopwatch.StartNew();

        //        const string query =
        //            "PREFIX at: <http://www.brightstardb.com/actors/>" +
        //            "SELECT ?Name WHERE {" +
        //                "?ator a at:Actor ." +
        //                "?ator at:Name ?Name ." +
        //            " }";
        //        var resultado = client.ExecuteQuery(storeName: "Filmes", queryExpression: query
        //            , resultsFormat: SparqlResultsFormat.Json
        //            , defaultGraphUri: "http://www.brightstardb.com/.well-known/model/defaultgraph"
        //            );

        //        {
        //            var buffer = new byte[resultado.Length];
        //            resultado.Read(buffer, 0, buffer.Length);
        //            File.WriteAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "teste.json"), buffer);
        //        }

        //        watch.Stop();
        //        Console.WriteLine(watch.Elapsed);
        //    }

        //    //var query = "SELECT ?category WHERE { " +
        //    //"<http://www.brightstardb.com/products/brightstar> <http://brightstardb.com/namespaces/default/name> ?category " +
        //    //"}";
        //}
    }
}
