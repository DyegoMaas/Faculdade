using Autofac;
using SimuladorSGBD.Core.GerenciamentoBuffer;
using SimuladorSGBD.IoC;
using System;
using System.Linq;

namespace SimuladorSGBD
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = ConfiguradorAufofac.Configurar();
            var gerenciadorBuffer = container.Resolve<IGerenciadorBuffer>();

            var pagina = gerenciadorBuffer.CarregarPagina(0);
            gerenciadorBuffer.AtualizarPagina(0, new String('f', 128).ToCharArray());
            var paginaMesma = gerenciadorBuffer.CarregarPagina(0);
            gerenciadorBuffer.SalvarPagina(0);
            var listaPaginasBuffer = gerenciadorBuffer.ListarPaginas().ToList();

            listaPaginasBuffer.ForEach(Console.WriteLine);
            
            Console.Read();
        }
    }
}
