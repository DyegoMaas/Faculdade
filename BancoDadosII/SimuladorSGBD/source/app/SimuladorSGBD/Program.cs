using System.Linq;
using Microsoft.Practices.ServiceLocation;
using SimuladorSGBD.Core.GerenciamentoBuffer;
using SimuladorSGBD.Core.GerenciamentoBuffer.Paginas;
using SimuladorSGBD.IoC;
using System;
using System.Collections.Generic;
using System.IO;

namespace SimuladorSGBD
{
    class Program
    {
        private const string NomeArquivoSaida = "relatorioBuffer.txt";
        private const string ComandoSair = "SAIR";
        private const string ComandoObter = "OBTER";
        private const string ComandoAtualizar = "ATUALIZAR";
        private const string ComandoImprimir = "IMPRIMIR";

        static void Main(string[] args)
        {
            ConfiguradorAufofac.Configurar();
            var gerenciadorBuffer = ServiceLocator.Current.GetInstance<IGerenciadorBuffer>();

            ImprimirTutorial();

            var executando = true;
            while (executando)
            {
                Console.WriteLine();
                Console.WriteLine("Insira o proximo comando:");

                var comando = LerComando();
                switch (comando.Nome)
                {
                    case ComandoSair:
                        executando = false;
                        break;
                    case ComandoObter:
                        var indicePaginaObter = comando.ObterInt(0);
                        gerenciadorBuffer.ObterPagina(indicePaginaObter);
                        break;
                    case ComandoAtualizar:
                        var indicePaginaAtualizar = comando.ObterInt(0);
                        var conteudo = comando.ObterString(1).ToCharArray();

                        var conteudoCompleto = Enumerable.Repeat('0', 128).ToArray();
                        Array.Copy(conteudo, conteudoCompleto, conteudo.Length);

                        gerenciadorBuffer.AtualizarPagina(indicePaginaAtualizar, conteudoCompleto);
                        break;
                    case ComandoImprimir:
                        gerenciadorBuffer.ListarPaginas()
                            .ToList()
                            .ForEach(Console.WriteLine);
                        break;
                }
            }

            Console.Read();
        }

        private static void ImprimirTutorial()
        {
            Console.WriteLine("Comandos disponiveis:");
            Console.WriteLine(string.Format("{0} (encerra o programa)", ComandoSair));
            Console.WriteLine(string.Format("{0} -INDICE_DA_PAGINA (carrega a pagina indicada)", ComandoObter));
            Console.WriteLine(string.Format("{0} -INDICE_DA_PAGINA -CONTEUDO_128_CARACTERES (atualiza a pagina indicada com o conteudo informado)", ComandoAtualizar));
            Console.WriteLine(string.Format("{0} (imprime um resumo do estado dos quadros no buffer)", ComandoImprimir));
            Console.WriteLine();
        }

        private static Comando LerComando()
        {
            var entrada = Console.ReadLine();
            if(entrada == null)
                return new Comando {Nome = string.Empty};

            var comandoString = entrada.Replace("-", string.Empty).Split(' ');
            return new Comando
            {
                Nome = comandoString.Length > 0 ? comandoString[0].ToUpper() : string.Empty,
                Parametros = comandoString.Skip(1).ToArray(),
            };
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

    internal class Comando
    {
        public string Nome { get; set; }
        public string[] Parametros { get; set; }

        public string ObterString(int indice)
        {
            return Parametros[indice];
        }

        public int ObterInt(int indice)
        {
            return int.Parse(ObterString(indice));
        }
    }
}
