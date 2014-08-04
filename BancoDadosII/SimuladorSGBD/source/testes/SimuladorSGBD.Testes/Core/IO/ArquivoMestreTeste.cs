using FluentAssertions;
using SimuladorSGBD.Core;
using SimuladorSGBD.Core.IO;
using System;
using System.IO;
using System.Threading;
using SimuladorSGBD.Testes.Fixtures;
using Xunit;

namespace SimuladorSGBD.Testes.Core.IO
{
    public class ArquivoMestreTeste
    {
        private const int TamanhoPaginas = 128;

        private readonly IConfiguracaoIO configuracaoDefault = new ConfiguracaoIO
        {
            CaminhoArquivoMestre = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "configuracaoDefault.txt")
        };

        public ArquivoMestreTeste()
        {
            TentarExcluirArquivo(3);
        }

        [Fact]
        public void criacao_de_um_arquivo_que_nao_existe()
        {
            var arquivoMestre = new ArquivoMestre(configuracaoDefault);
            arquivoMestre.CriarArquivoSeNaoExiste(1, 1);

            File.Exists(configuracaoDefault.CaminhoArquivoMestre).Should().BeTrue("deveria ter criado o arquivo master");
        }

        [Fact]
        public void verificando_se_arquivo_existe_quando_nao_existe()
        {
            var arquivoMestre = new ArquivoMestre(configuracaoDefault);
            arquivoMestre.ExisteNoDisco.Should().BeFalse("o arquivo não existe");
        }

        [Fact]
        public void verificando_se_arquivo_existe_quando_existe()
        {
            var arquivo = new FileInfo(configuracaoDefault.CaminhoArquivoMestre);
            using (arquivo.Create()){}

            var arquivoMestre = new ArquivoMestre(configuracaoDefault);
            arquivoMestre.ExisteNoDisco.Should().BeTrue("o arquivo existe");
        }

        [Fact]
        public void criacao_de_blocos_na_inicilizacao()
        {
            const int numeroBlocos = 2;

            var arquivoMestre = new ArquivoMestre(configuracaoDefault);
            arquivoMestre.CriarArquivoSeNaoExiste(numeroBlocos, TamanhoPaginas);

            var bytesArquivo = File.ReadAllBytes(configuracaoDefault.CaminhoArquivoMestre);
            bytesArquivo.Length.Should().Be(numeroBlocos * TamanhoPaginas);
        }

        [Fact]
        public void carregando_uma_pagina_do_disco()
        {
            DadoQueExisteUmArquivoComDuasPaginas(tamanhoPaginas: TamanhoPaginas, conteudoPrimeiro: 'a', conteudoSegundo: 'b');
            var arquivoMestre = new ArquivoMestre(configuracaoDefault);

            APaginaDeveConterApenas(arquivoMestre, indicePagina: 0, caractere: 'a');
            APaginaDeveConterApenas(arquivoMestre, indicePagina: 1, caractere: 'b');
        }

        [Fact]
        public void salvando_uma_pagina_no_disco()
        {
            DadoQueExisteUmArquivoComDuasPaginas(tamanhoPaginas: TamanhoPaginas, conteudoPrimeiro: 'a', conteudoSegundo: 'b');
            var arquivoMestre = new ArquivoMestre(configuracaoDefault);

            arquivoMestre.SalvarPagina(0, NovaPagina(TamanhoPaginas, 'c'));

            APaginaDeveConterApenas(arquivoMestre, indicePagina:0, caractere:'c');
            APaginaDeveConterApenas(arquivoMestre, indicePagina:1, caractere:'b');
        }
        
        private void DadoQueExisteUmArquivoComDuasPaginas(int tamanhoPaginas, char conteudoPrimeiro, char conteudoSegundo)
        {
            var arquivo = new FileInfo(configuracaoDefault.CaminhoArquivoMestre);
            using (var streamWriter = arquivo.CreateText())
            {
                EscreverUmaPagina(streamWriter, NovaPagina(tamanho: TamanhoPaginas, preenchidaCom: conteudoPrimeiro));
                EscreverUmaPagina(streamWriter, NovaPagina(tamanho: TamanhoPaginas, preenchidaCom: conteudoSegundo));
            }
        }

        private IPaginaComConteudo NovaPagina(int tamanho, char preenchidaCom)
        {
            return new PaginaTesteBuilder().PreenchidoCom(tamanho, preenchidaCom).Construir();
        }

        private void EscreverUmaPagina(StreamWriter streamWriter, IPaginaComConteudo pagina)
        {
            streamWriter.Write(pagina.Conteudo);
        }

        private static void APaginaDeveConterApenas(ArquivoMestre arquivoMestre, int indicePagina, char caractere)
        {
            IPaginaComConteudo paginaUm = arquivoMestre.CarregarPagina(indicePagina);
            foreach (var c in paginaUm.Conteudo)
            {
                c.Should().Be(caractere);
            }
        }

        private void TentarExcluirArquivo(int numeroTentativas)
        {
            var arquivo = new FileInfo(configuracaoDefault.CaminhoArquivoMestre);
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