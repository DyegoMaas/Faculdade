using FluentAssertions;
using SimuladorSGBD.Core;
using SimuladorSGBD.Core.IO;
using System;
using System.IO;
using System.Threading;
using SimuladorSGBD.Testes.Core.ArmazenamentoRegistros;
using SimuladorSGBD.Testes.Fixtures;
using Xunit;

namespace SimuladorSGBD.Testes.Core.IO
{
    public class GerenciadorEspacoEmDiscoTeste
    {
        private const int TamanhoPaginas = 128;
        private ConteudoPaginaTesteHelper conteudoPaginaTesteHelper;

        private readonly IConfiguracaoIO configuracaoDefault = new ConfiguracaoIO
        {
            CaminhoArquivoMestre = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "configuracaoDefault.txt")
        };

        public GerenciadorEspacoEmDiscoTeste()
        {
            TentarExcluirArquivo(3);
            conteudoPaginaTesteHelper = new ConteudoPaginaTesteHelper();
        }

        [Fact]
        public void criacao_de_um_arquivo_que_nao_existe()
        {
            var arquivoMestre = new GerenciadorEspacoEmDisco(configuracaoDefault);
            arquivoMestre.CriarArquivoSeNaoExiste(1, 1);

            File.Exists(configuracaoDefault.CaminhoArquivoMestre).Should().BeTrue("deveria ter criado o arquivo master");
        }

        [Fact]
        public void verificando_se_arquivo_existe_quando_nao_existe()
        {
            var arquivoMestre = new GerenciadorEspacoEmDisco(configuracaoDefault);
            arquivoMestre.ExisteNoDisco.Should().BeFalse("o arquivo não existe");
        }

        [Fact]
        public void verificando_se_arquivo_existe_quando_existe()
        {
            var arquivo = new FileInfo(configuracaoDefault.CaminhoArquivoMestre);
            using (arquivo.Create()){}

            var arquivoMestre = new GerenciadorEspacoEmDisco(configuracaoDefault);
            arquivoMestre.ExisteNoDisco.Should().BeTrue("o arquivo existe");
        }

        [Fact]
        public void criacao_de_blocos_na_inicilizacao()
        {
            const int numeroBlocos = 2;

            var arquivoMestre = new GerenciadorEspacoEmDisco(configuracaoDefault);
            arquivoMestre.CriarArquivoSeNaoExiste(numeroBlocos, TamanhoPaginas);

            var bytesArquivo = File.ReadAllBytes(configuracaoDefault.CaminhoArquivoMestre);
            bytesArquivo.Length.Should().Be(numeroBlocos * TamanhoPaginas);
        }

        [Fact]
        public void carregando_uma_pagina_do_disco()
        {
            DadoQueExisteUmArquivoComDuasPaginas(tamanhoPaginas: TamanhoPaginas, conteudoPrimeiro: 'a', conteudoSegundo: 'b');
            var arquivoMestre = new GerenciadorEspacoEmDisco(configuracaoDefault);

            APaginaDeveConterApenas(arquivoMestre, indicePagina: 0, caractere: 'a');
            APaginaDeveConterApenas(arquivoMestre, indicePagina: 1, caractere: 'b');
        }

        [Fact]
        public void salvando_uma_pagina_no_disco()
        {
            DadoQueExisteUmArquivoComDuasPaginas(tamanhoPaginas: TamanhoPaginas, conteudoPrimeiro: 'a', conteudoSegundo: 'b');
            var arquivoMestre = new GerenciadorEspacoEmDisco(configuracaoDefault);

            arquivoMestre.SalvarPagina(0, NovaPagina(TamanhoPaginas, 'c'));

            APaginaDeveConterApenas(arquivoMestre, indicePagina:0, caractere:'c');
            APaginaDeveConterApenas(arquivoMestre, indicePagina:1, caractere:'b');
        }
        
        private void DadoQueExisteUmArquivoComDuasPaginas(int tamanhoPaginas, char conteudoPrimeiro, char conteudoSegundo)
        {
            var arquivo = new FileInfo(configuracaoDefault.CaminhoArquivoMestre);
            using (var fileStream = arquivo.Create())
            {
                EscreverUmaPagina(fileStream, NovaPagina(tamanho: TamanhoPaginas, preenchidaCom: conteudoPrimeiro));
                EscreverUmaPagina(fileStream, NovaPagina(tamanho: TamanhoPaginas, preenchidaCom: conteudoSegundo));
            }
        }

        private IPagina NovaPagina(int tamanho, char preenchidaCom)
        {
            return new QuadroTesteBuilder().PreenchidoCom(tamanho, preenchidaCom).Construir().Pagina;
        }

        private void EscreverUmaPagina(FileStream streamWriter, IPagina pagina)
        {
            streamWriter.Write(pagina.Conteudo, 0, pagina.Conteudo.Length);
        }

        private void APaginaDeveConterApenas(GerenciadorEspacoEmDisco gerenciadorEspacoEmDisco, int indicePagina, char caractere)
        {
            var @byte = conteudoPaginaTesteHelper.ToByte(caractere);

            IPagina paginaUm = gerenciadorEspacoEmDisco.CarregarPagina(indicePagina);
            foreach (var c in paginaUm.Conteudo)
            {
                c.Should().Be(@byte);
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