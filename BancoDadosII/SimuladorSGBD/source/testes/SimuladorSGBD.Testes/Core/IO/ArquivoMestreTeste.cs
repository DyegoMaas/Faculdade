using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using FluentAssertions;
using SimuladorSGBD.Core;
using SimuladorSGBD.Core.IO;
using Xunit;
using Xunit.Extensions;

namespace SimuladorSGBD.Testes.Core.IO
{
    public class ArquivoMestreTeste
    {
        private readonly string arquivoTeste = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "arquivoTeste.txt");

        public ArquivoMestreTeste()
        {
            TentarExcluirArquivo(3);
        }

        [Fact]
        public void criacao_de_um_arquivo_que_nao_existe()
        {
            var manipuladorArquivos = new ArquivoMestre(arquivoTeste);
            manipuladorArquivos.CriarArquivoSeNaoExiste(1, 1);

            File.Exists(arquivoTeste).Should().BeTrue("deveria ter criado o arquivo master");
        }

        [Fact]
        public void verificando_se_arquivo_existe_quando_nao_existe()
        {
            var manipuladorArquivos = new ArquivoMestre(arquivoTeste);
            manipuladorArquivos.ExisteNoDisco.Should().BeFalse("o arquivo não existe");
        }

        [Fact]
        public void verificando_se_arquivo_existe_quando_existe()
        {
            var arquivo = new FileInfo(arquivoTeste);
            using (arquivo.Create()){}

            var manipuladorArquivos = new ArquivoMestre(arquivoTeste);
            manipuladorArquivos.ExisteNoDisco.Should().BeTrue("o arquivo existe");
        }

        [Fact]
        public void criacao_de_blocos_na_inicilizacao()
        {
            var manipuladorArquivos = new ArquivoMestre(arquivoTeste);
            manipuladorArquivos.CriarArquivoSeNaoExiste(2, 128);

            var bytesArquivo = File.ReadAllBytes(arquivoTeste);
            bytesArquivo.Length.Should().Be(256);
        }

        [Fact]
        public void carregando_uma_pagina_do_disco()
        {
            DadoQueExisteUmArquivoComDuasPaginas(tamanhoPaginas:128, conteudoPrimeiro:'a', conteudoSegundo:'b');
            var manipuladorArquivos = new ArquivoMestre(arquivoTeste);

            var paginaUm = manipuladorArquivos.CarregarPagina(0);
            APaginaContemApenas(paginaUm, caractereEsperado:'a');

            var paginaDois = manipuladorArquivos.CarregarPagina(1);
            APaginaContemApenas(paginaDois, caractereEsperado: 'b');
        }

        private static void APaginaContemApenas(IPagina paginaUm, char caractereEsperado)
        {
            foreach (var caractere in paginaUm.Dados)
            {
                caractere.Should().Be(caractereEsperado);
            }
        }

        private void DadoQueExisteUmArquivoComDuasPaginas(int tamanhoPaginas, char conteudoPrimeiro, char conteudoSegundo)
        {
            var arquivo = new FileInfo(arquivoTeste);
            using (var streamWriter = arquivo.CreateText())
            {
                EscreverUmaPagina(streamWriter, NovaPagina(tamanho: 128, preenchidoCom: conteudoPrimeiro));
                EscreverUmaPagina(streamWriter, NovaPagina(tamanho: 128, preenchidoCom: conteudoSegundo));
            }
        }

        private string NovaPagina(int tamanho, char preenchidoCom)
        {
            return new string(preenchidoCom, tamanho);
        }

        private void EscreverUmaPagina(StreamWriter streamWriter, string conteudo)
        {
            streamWriter.Write(conteudo);
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