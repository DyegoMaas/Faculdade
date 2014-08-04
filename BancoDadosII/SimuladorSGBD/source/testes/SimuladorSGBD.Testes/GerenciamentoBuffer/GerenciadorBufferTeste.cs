using FluentAssertions;
using Moq;
using SimuladorSGBD.Core;
using SimuladorSGBD.Core.GerenciamentoBuffer;
using SimuladorSGBD.Core.IO;
using SimuladorSGBD.Testes.Fixtures;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;
using Xunit.Extensions;

namespace SimuladorSGBD.Testes.GerenciamentoBuffer
{
    public class GerenciadorBufferTeste
    {
        private const int IndiceZero = 0;
        private const int IndiceUm = 1;
        private const int IndiceDois = 2;

        [Theory,
        InlineData(0),
        InlineData(2),
        InlineData(10)]
        public void carregando_uma_pagina_para_o_buffer(int indicePagina)
        {
            var mockArquivoMestre = new Mock<IArquivoMestre>();
            var mockBuffer = new Mock<IBufferEmMemoria>();

            var paginaNoDisco = new PaginaTesteBuilder().PreenchidoCom(128, 'a').Construir();
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
            var mockArquivoMestre = new Mock<IArquivoMestre>();
            var mockBuffer = new Mock<IBufferEmMemoria>();

            var paginaNoBuffer = new PaginaTesteBuilder().NoIndice(IndiceUm).Construir();
            mockBuffer.Setup(buffer => buffer.Obter(IndiceUm)).Returns(paginaNoBuffer);

            var gerenciadorBuffer = new GerenciadorBuffer(mockArquivoMestre.Object, mockBuffer.Object, UmaConfiguracaoDeBuffer(1));
            gerenciadorBuffer.SalvarPagina(IndiceUm);

            mockBuffer.Verify(b => b.Obter(IndiceUm));
            mockArquivoMestre.Verify(a => a.SalvarPagina(IndiceUm, It.Is<IPaginaComConteudo>(p => p == paginaNoBuffer)), Times.Once, "deveria salvar a página no disco");
        }

        //integração
        [Fact]
        public void restringindo_o_buffer_ao_numero_limite_de_paginas_configuradas()
        {
            var mockArquivoMestre = new Mock<IArquivoMestre>();
            DadoQueExisteUmaPaginaEmDiscoNoIndice(mockArquivoMestre, IndiceZero);
            DadoQueExisteUmaPaginaEmDiscoNoIndice(mockArquivoMestre, IndiceUm);
            DadoQueExisteUmaPaginaEmDiscoNoIndice(mockArquivoMestre, IndiceDois);

            var gerenciadorBuffer = new GerenciadorBuffer(mockArquivoMestre.Object, new BufferEmMemoria(),
                UmaConfiguracaoDeBuffer(limiteDePaginasEmMemoria:2));
            gerenciadorBuffer.CarregarPagina(IndiceZero);
            gerenciadorBuffer.CarregarPagina(IndiceUm);

            gerenciadorBuffer.Invoking(g => g.CarregarPagina(IndiceDois)).ShouldThrow<InvalidOperationException>();
        }

        //integração
        [Fact]
        public void alterando_uma_pagina_em_memoria()
        {
            const int tamanhoConteudo = 128;

            var mockArquivoMestre = new Mock<IArquivoMestre>();
            var buffer = new BufferEmMemoria();
            var gerenciadorBuffer = new GerenciadorBuffer(mockArquivoMestre.Object, buffer, UmaConfiguracaoDeBuffer(1));

            var pagina = new PaginaTesteBuilder().PreenchidoCom(numeroCaracteres: tamanhoConteudo, caractere: 'a').Construir();
            mockArquivoMestre.Setup(a => a.CarregarPagina(IndiceUm)).Returns(pagina);
            gerenciadorBuffer.CarregarPagina(IndiceUm);

            gerenciadorBuffer.AtualizarPagina(IndiceUm, ConteudoPaginaTesteHelper.NovoConteudo(tamanhoConteudo, 'x'));

            var paginaRecuperada = buffer.Obter(IndiceUm);
            paginaRecuperada.Sujo.Should().BeTrue("deve marcar uma página como suja ao atualizar seu conteúdo");
            APaginaDeveConterApenas(paginaRecuperada, 'x');
        }

        [Fact]
        public void carregando_pagina_do_buffer_se_ela_ja_estiver_nele()
        {
            var pagina = new PaginaTesteBuilder().NoIndice(IndiceUm).Construir();

            var mockArquivoMestre = new Mock<IArquivoMestre>();
            var mockBuffer = new Mock<IBufferEmMemoria>();
            mockBuffer.Setup(buffer => buffer.Obter(IndiceUm)).Returns(pagina);

            var gerenciadorBuffer = new GerenciadorBuffer(mockArquivoMestre.Object, mockBuffer.Object, UmaConfiguracaoDeBuffer(1));
            var paginaRecuperada = gerenciadorBuffer.CarregarPagina(IndiceUm);

            mockBuffer.Verify(b => b.Obter(IndiceUm), Times.Once);
            mockArquivoMestre.Verify(b => b.CarregarPagina(IndiceUm), Times.Never);

            paginaRecuperada.IndicePaginaNoDisco.Should().Be(IndiceUm);
        }

        [Fact]
        public void listando_as_paginas_no_buffer()
        {
            var mockBuffer = new Mock<IBufferEmMemoria>();
            mockBuffer.Setup(b => b.ListarPaginas()).Returns(new IResumoPagina[2]);

            var gerenciadorBuffer = new GerenciadorBuffer(new Mock<IArquivoMestre>().Object, mockBuffer.Object, UmaConfiguracaoDeBuffer(3));
            IEnumerable<IResumoPagina> resumoBuffer = gerenciadorBuffer.ListarPaginas();

            mockBuffer.Verify(b => b.ListarPaginas());
            resumoBuffer.Should().HaveCount(2, "deveria listar os três itens do buffer");
        }

        private static void DadoQueExisteUmaPaginaEmDiscoNoIndice(Mock<IArquivoMestre> mockArquivoMestre, int indicePagina)
        {
            mockArquivoMestre.Setup(buffer => buffer.CarregarPagina(indicePagina))
                .Returns(new PaginaTesteBuilder().NoIndice(indicePagina).Construir);
        }

        private IConfiguracaoBuffer UmaConfiguracaoDeBuffer(int limiteDePaginasEmMemoria)
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
}