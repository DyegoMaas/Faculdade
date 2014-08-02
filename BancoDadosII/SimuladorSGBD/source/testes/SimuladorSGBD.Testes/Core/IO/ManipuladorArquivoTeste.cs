using System;
using System.IO;
using FluentAssertions;
using SimuladorSGBD.Core.IO;
using Xunit;

namespace SimuladorSGBD.Testes.Core.IO
{
    public class ManipuladorArquivoTeste
    {
        private readonly string arquivoTeste = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "arquivoTeste.txt");

        public ManipuladorArquivoTeste()
        {
            var arquivo = new FileInfo(arquivoTeste);
            if(arquivo.Exists)
                arquivo.Delete();
        }

        [Fact]
        public void criacao_de_um_arquivo_que_nao_existe()
        {
            var manipuladorArquivos = new ManipuladorArquivo(arquivoTeste);
            manipuladorArquivos.CriarArquivoSeNaoExiste(1, 1);

            File.Exists(arquivoTeste).Should().BeTrue("deveria ter criado o arquivo master");
        }

        [Fact]
        public void verificando_se_arquivo_existe_quando_nao_existe()
        {
            var manipuladorArquivos = new ManipuladorArquivo(arquivoTeste);
            manipuladorArquivos.ArquivoExiste().Should().BeFalse("o arquivo não existe");
        }

        [Fact]
        public void verificando_se_arquivo_existe_quando_existe()
        {
            var arquivo = new FileInfo(arquivoTeste);
            arquivo.Create();

            var manipuladorArquivos = new ManipuladorArquivo(arquivoTeste);
            manipuladorArquivos.ArquivoExiste().Should().BeTrue("o arquivo existe");
        }

        [Fact]
        public void criacao_de_blocos_na_inicilizacao()
        {
            var manipuladorArquivos = new ManipuladorArquivo(arquivoTeste);
            manipuladorArquivos.CriarArquivoSeNaoExiste(2, 128);

            var bytesArquivo = File.ReadAllBytes(arquivoTeste);
            bytesArquivo.Length.Should().Be(256);
        }
    }
}