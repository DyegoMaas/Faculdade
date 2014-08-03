using System;
using System.Linq;
using FluentAssertions;
using Moq;
using SimuladorSGBD.Core;
using SimuladorSGBD.Core.GerenciamentoBuffer;
using SimuladorSGBD.Core.IO;
using SimuladorSGBD.Testes.Fixtures;
using Xunit;
using Xunit.Extensions;

namespace SimuladorSGBD.Testes.GerenciamentoBuffer
{
    public class GerenciadorBufferTeste
    {
        [Theory,
        InlineData(0),
        InlineData(2),
        InlineData(10)]
        public void carregando_uma_pagina_para_o_buffer(int indicePagina)
        {
            var mockArquivoMestre = new Mock<IArquivoMestre>();
            var mockBuffer = new Mock<IBufferEmMemoria>();

            var paginaNoDisco = new PaginaFake { Conteudo = new char[128] };
            mockArquivoMestre.Setup(m => m.CarregarPagina(indicePagina)).Returns(paginaNoDisco);

            var gerenciadorBuffer = new GerenciadorBuffer(mockArquivoMestre.Object, mockBuffer.Object, UmaConfiguracaoDeBuffer(1));
            IPaginaEmMemoria pagina = gerenciadorBuffer.CarregarPagina(indicePagina);
            pagina.Conteudo.Should().HaveSameCount(paginaNoDisco.Conteudo);
            pagina.Sujo.Should().Be(false);
            pagina.PinCount.Should().Be(0);
            pagina.UltimoAcesso.Should().Be(0);
            pagina.IndicePaginaNoDisco.Should().Be(indicePagina);

            mockArquivoMestre.Verify(m => m.CarregarPagina(indicePagina));
            mockBuffer.Verify(buffer => buffer.Armazenar(It.Is<IPaginaEmMemoria>(p => p.IndicePaginaNoDisco == indicePagina)));
        }

        [Fact]
        public void salvando_uma_pagina_no_disco()
        {
            const int indiceUm = 1;

            var mockArquivoMestre = new Mock<IArquivoMestre>();
            var mockBuffer = new Mock<IBufferEmMemoria>();

            var paginaNoBuffer = new PaginaTesteBuilder().NoIndice(indiceUm).Construir();
            mockBuffer.Setup(buffer => buffer.Obter(indiceUm)).Returns(paginaNoBuffer);

            var gerenciadorBuffer = new GerenciadorBuffer(mockArquivoMestre.Object, mockBuffer.Object, UmaConfiguracaoDeBuffer(1));
            gerenciadorBuffer.SalvarPagina(indiceUm);

            mockBuffer.Verify(b => b.Obter(indiceUm));
            mockArquivoMestre.Verify(a => a.SalvarPagina(indiceUm, It.Is<IPaginaComConteudo>(p => p == paginaNoBuffer)), Times.Once, "deveria salvar a página no disco");
        }

        //integração
        [Fact]
        public void restringindo_o_buffer_ao_numero_limite_de_paginas_configuradas()
        {
            const int indiceZero = 0;
            const int indiceUm = 1;
            const int indiceDois = 2;

            var mockArquivoMestre = new Mock<IArquivoMestre>();
            DadoQueExisteUmaPaginaEmDiscoNoIndice(mockArquivoMestre, indiceZero);
            DadoQueExisteUmaPaginaEmDiscoNoIndice(mockArquivoMestre, indiceUm);
            DadoQueExisteUmaPaginaEmDiscoNoIndice(mockArquivoMestre, indiceDois);

            var gerenciadorBuffer = new GerenciadorBuffer(mockArquivoMestre.Object, new BufferEmMemoria(),
                UmaConfiguracaoDeBuffer(limiteDePaginasEmMemoria:2));
            gerenciadorBuffer.CarregarPagina(indiceZero);
            gerenciadorBuffer.CarregarPagina(indiceUm);

            gerenciadorBuffer.Invoking(g => g.CarregarPagina(indiceDois)).ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void alterando_uma_pagina_em_memoria()
        {
            const int indiceUm = 1;
            const int tamanhoConteudo = 128;

            var mockArquivoMestre = new Mock<IArquivoMestre>();
            var buffer = new BufferEmMemoria();
            var gerenciadorBuffer = new GerenciadorBuffer(mockArquivoMestre.Object, buffer, UmaConfiguracaoDeBuffer(1));

            var pagina = new PaginaTesteBuilder().PreenchidoCom(numeroCaracteres: tamanhoConteudo, caractere: 'a').Construir();
            mockArquivoMestre.Setup(a => a.CarregarPagina(indiceUm)).Returns(pagina);
            gerenciadorBuffer.CarregarPagina(indiceUm);

            gerenciadorBuffer.AtualizarPagina(indiceUm, ConteudoPaginaTesteHelper.NovoConteudo(tamanhoConteudo, 'x'));

            var paginaRecuperada = buffer.Obter(indiceUm);
            paginaRecuperada.Sujo.Should().BeTrue("deve marcar uma página como suja ao atualizar seu conteúdo");
            APaginaDeveConterApenas(paginaRecuperada, 'x');
        }

        private static void DadoQueExisteUmaPaginaEmDiscoNoIndice(Mock<IArquivoMestre> mockArquivoMestre, int indicePagina)
        {
            mockArquivoMestre.Setup(buffer => buffer.CarregarPagina(indicePagina))
                .Returns(new PaginaTesteBuilder().NoIndice(indicePagina).Construir);
        }

        private IConfiguaracaoBuffer UmaConfiguracaoDeBuffer(int limiteDePaginasEmMemoria)
        {
            return new ConfiguracaoBuffer
            {
                LimiteDePaginasEmMemoria = limiteDePaginasEmMemoria
            };
        }

        public void APaginaDeveConterApenas(IPaginaEmMemoria pagina, char caractere)
        {
            foreach (var c in pagina.Conteudo)
            {
                c.Should().Be(caractere);
            }
        }
    }

    public static class ConteudoPaginaTesteHelper
    {


        public static char[] NovoConteudo(int numeroCaracteres, char caractere)
        {
            return Enumerable.Repeat(caractere, numeroCaracteres).ToArray();
        }
    }
}