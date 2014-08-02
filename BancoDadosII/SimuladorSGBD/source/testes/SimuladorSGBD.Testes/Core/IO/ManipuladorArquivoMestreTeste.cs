using System;
using System.IO;
using System.Threading;
using FluentAssertions;
using SimuladorSGBD.Core.IO;
using Xunit;

namespace SimuladorSGBD.Testes.Core.IO
{
    public class ManipuladorArquivoMestreTeste
    {
        private readonly string arquivoTeste = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "arquivoTeste.txt");

        public ManipuladorArquivoMestreTeste()
        {
            TentarExcluirArquivo(3);
        }

        [Fact]
        public void criacao_de_um_arquivo_que_nao_existe()
        {
            var manipuladorArquivos = new ManipuladorArquivoMestreMestre(arquivoTeste);
            manipuladorArquivos.CriarArquivoSeNaoExiste(1, 1);

            File.Exists(arquivoTeste).Should().BeTrue("deveria ter criado o arquivo master");
        }

        [Fact]
        public void verificando_se_arquivo_existe_quando_nao_existe()
        {
            var manipuladorArquivos = new ManipuladorArquivoMestreMestre(arquivoTeste);
            manipuladorArquivos.ArquivoExiste().Should().BeFalse("o arquivo não existe");
        }

        [Fact]
        public void verificando_se_arquivo_existe_quando_existe()
        {
            var arquivo = new FileInfo(arquivoTeste);
            arquivo.Create();

            var manipuladorArquivos = new ManipuladorArquivoMestreMestre(arquivoTeste);
            manipuladorArquivos.ArquivoExiste().Should().BeTrue("o arquivo existe");
        }

        [Fact]
        public void criacao_de_blocos_na_inicilizacao()
        {
            var manipuladorArquivos = new ManipuladorArquivoMestreMestre(arquivoTeste);
            manipuladorArquivos.CriarArquivoSeNaoExiste(2, 128);

            var bytesArquivo = File.ReadAllBytes(arquivoTeste);
            bytesArquivo.Length.Should().Be(256);
        }

        private void TentarExcluirArquivo(int numeroTentativas)
        {
            var arquivo = new FileInfo(arquivoTeste);
            for (int i = 0; i < numeroTentativas; i++)
            {
                try
                {
                    if (arquivo.Exists)
                        arquivo.Delete();
                }
                catch
                {
                    Thread.Sleep(100);
                }
            }
        }
    }
}