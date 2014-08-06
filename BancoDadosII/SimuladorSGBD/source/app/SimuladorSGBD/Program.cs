using Microsoft.Practices.ServiceLocation;
using SimuladorSGBD.Core.GerenciamentoBuffer;
using SimuladorSGBD.Core.GerenciamentoBuffer.Paginas;
using SimuladorSGBD.IoC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SimuladorSGBD
{
    class Program
    {
        private const string NomeArquivoSaida = "relatorioBuffer.txt";

        static void Main(string[] args)
        {
            ConfiguradorAufofac.Configurar();
            var gerenciadorBuffer = ServiceLocator.Current.GetInstance<IGerenciadorBuffer>();

            var pagina = gerenciadorBuffer.ObterPagina(0);
            gerenciadorBuffer.AtualizarPagina(0, new String('f', 128).ToCharArray());
            var paginaMesma = gerenciadorBuffer.ObterPagina(0);
            gerenciadorBuffer.SalvarPagina(0);
            var listaPaginasBuffer = gerenciadorBuffer.ListarPaginas().ToList();

            CriarArquivoRepresentandoBuffer(listaPaginasBuffer);
            Console.Write(string.Format("Criado arquivo {0} com o conteúdo do buffer.", NomeArquivoSaida));
            Console.Read();
        }

        private static void CriarArquivoRepresentandoBuffer(List<IResumoPagina> listaPaginasBuffer)
        {
            var caminhoArquivoSaida = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, NomeArquivoSaida);
            using (var saida = File.CreateText(caminhoArquivoSaida))
            {
                listaPaginasBuffer.ForEach(p => saida.WriteLine(p.ToString()));
            }
        }
    }
}
