using System;
using System.IO;
using System.Runtime.InteropServices;
using FluentAssertions;
using SimuladorSGBD.Core;
using Xunit;
using Xunit.Extensions;

namespace SimuladorSGBD.Testes.Core
{
    public class ManipuladorArquivosTeste
    {
        private readonly string arquivoTeste = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "arquivoTeste.txt");

        public ManipuladorArquivosTeste()
        {
            var arquivo = new FileInfo(arquivoTeste);
            if(arquivo.Exists)
                arquivo.Delete();
        }

        [Fact]
        public void criacao_de_um_arquivo_que_nao_existe()
        {
            var manipuladorArquivos = new ManipuladorArquivos();
            manipuladorArquivos.CriarArquivo(arquivoTeste);

            File.Exists(arquivoTeste).Should().BeTrue("deveria ter criado o arquivo master");
        }

        [Fact]
        public void verificando_se_arquivo_existe_quando_nao_existe()
        {
            var manipuladorArquivos = new ManipuladorArquivos();
            manipuladorArquivos.ArquivoExiste(arquivoTeste).Should().BeFalse("o arquivo não existe");
        }

        [Fact]
        public void verificando_se_arquivo_existe_quando_existe()
        {
            var arquivo = new FileInfo(arquivoTeste);
            arquivo.Create();

            var manipuladorArquivos = new ManipuladorArquivos();
            manipuladorArquivos.ArquivoExiste(arquivoTeste).Should().BeTrue("o arquivo existe");
        }

        [Fact]
        public void criacao_de_um_novo_bloco()
        {
            var manipuladorArquivos = new ManipuladorArquivos();
            manipuladorArquivos.CriarArquivo(arquivoTeste);

            manipuladorArquivos.CriarBlocoVazio(128);

            using (var x = new FileStream())
            {
                
            }
        }
    }
}