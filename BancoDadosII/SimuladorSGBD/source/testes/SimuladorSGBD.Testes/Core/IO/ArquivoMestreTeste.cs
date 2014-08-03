using FluentAssertions;
using SimuladorSGBD.Core;
using SimuladorSGBD.Core.IO;
using System;
using System.IO;
using System.Threading;
using Xunit;

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
            var arquivoMestre = new ArquivoMestre(arquivoTeste);
            arquivoMestre.CriarArquivoSeNaoExiste(1, 1);

            File.Exists(arquivoTeste).Should().BeTrue("deveria ter criado o arquivo master");
        }

        [Fact]
        public void verificando_se_arquivo_existe_quando_nao_existe()
        {
            var arquivoMestre = new ArquivoMestre(arquivoTeste);
            arquivoMestre.ExisteNoDisco.Should().BeFalse("o arquivo não existe");
        }

        [Fact]
        public void verificando_se_arquivo_existe_quando_existe()
        {
            var arquivo = new FileInfo(arquivoTeste);
            using (arquivo.Create()){}

            var arquivoMestre = new ArquivoMestre(arquivoTeste);
            arquivoMestre.ExisteNoDisco.Should().BeTrue("o arquivo existe");
        }

        [Fact]
        public void criacao_de_blocos_na_inicilizacao()
        {
            var arquivoMestre = new ArquivoMestre(arquivoTeste);
            arquivoMestre.CriarArquivoSeNaoExiste(2, 128);

            var bytesArquivo = File.ReadAllBytes(arquivoTeste);
            bytesArquivo.Length.Should().Be(256);
        }

        [Fact]
        public void carregando_uma_pagina_do_disco()
        {
            DadoQueExisteUmArquivoComDuasPaginas(tamanhoPaginas:128, conteudoPrimeiro:'a', conteudoSegundo:'b');
            var arquivoMestre = new ArquivoMestre(arquivoTeste);

            IPaginaComDados paginaUm = arquivoMestre.CarregarPagina(0);
            APaginaDeveConterApenas(paginaUm, caractereEsperado:'a');

            IPaginaComDados paginaDois = arquivoMestre.CarregarPagina(1);
            APaginaDeveConterApenas(paginaDois, caractereEsperado: 'b');
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

        private static void APaginaDeveConterApenas(IPaginaComDados paginaUm, char caractereEsperado)
        {
            foreach (var caractere in paginaUm.Dados)
            {
                caractere.Should().Be(caractereEsperado);
            }
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