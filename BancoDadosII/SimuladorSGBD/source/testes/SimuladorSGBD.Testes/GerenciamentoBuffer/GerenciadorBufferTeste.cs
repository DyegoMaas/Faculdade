using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using SimuladorSGBD.Core;
using SimuladorSGBD.Core.GerenciamentoBuffer;
using SimuladorSGBD.Core.GerenciamentoBuffer.Buffer;
using SimuladorSGBD.Core.GerenciamentoBuffer.Paginas;
using SimuladorSGBD.Core.IO;
using SimuladorSGBD.Testes.Fixtures;
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
            var mockGerenciadorDisco = new Mock<IGerenciadorEspacoEmDisco>();
            var mockBuffer = new Mock<IPoolDeBuffers>();

            var quadroDisco = new QuadroTesteBuilder().PreenchidoCom(128, 'a').Construir();
            mockGerenciadorDisco.Setup(m => m.CarregarPagina(indicePagina)).Returns(quadroDisco.Pagina);

            var gerenciadorBuffer = new GerenciadorBuffer(mockGerenciadorDisco.Object, mockBuffer.Object, UmaConfiguracaoDeBuffer(1));
            IQuadro quadro = gerenciadorBuffer.LerPagina(indicePagina);
            quadro.Pagina.Conteudo.Should().HaveSameCount(quadroDisco.Pagina.Conteudo);
            quadro.Sujo.Should().Be(false);
            quadro.PinCount.Should().Be(0);
            quadro.UltimoAcesso.Should().Be(0);
            quadro.IndicePaginaNoDisco.Should().Be(indicePagina);

            mockGerenciadorDisco.Verify(m => m.CarregarPagina(indicePagina));
            mockBuffer.Verify(buffer => buffer.Armazenar(It.Is<IQuadro>(p => p.IndicePaginaNoDisco == indicePagina)));
        }

        [Fact]
        public void salvando_uma_pagina_no_disco()
        {
            var mockGerenciadorDisco = new Mock<IGerenciadorEspacoEmDisco>();
            var mockBuffer = new Mock<IPoolDeBuffers>();

            var quadroNoBuffer = new QuadroTesteBuilder().NoIndice(IndiceUm).Construir();
            mockBuffer.Setup(buffer => buffer.Obter(IndiceUm)).Returns(quadroNoBuffer);

            var gerenciadorBuffer = new GerenciadorBuffer(mockGerenciadorDisco.Object, mockBuffer.Object, UmaConfiguracaoDeBuffer(1));
            gerenciadorBuffer.SalvarPagina(IndiceUm);

            mockBuffer.Verify(b => b.Obter(IndiceUm));
            mockGerenciadorDisco.Verify(a => a.SalvarPagina(IndiceUm, It.Is<IPagina>(p => p == quadroNoBuffer.Pagina)), Times.Once, "deveria salvar a página no disco");
        }
        
        //integração
        [Fact]
        public void alterando_uma_pagina_em_memoria()
        {
            const int tamanhoConteudo = 128;

            var mockGerenciadorDisco = new Mock<IGerenciadorEspacoEmDisco>();
            var buffer = new PoolDeBuffers();
            var gerenciadorBuffer = new GerenciadorBuffer(mockGerenciadorDisco.Object, buffer, UmaConfiguracaoDeBuffer(1));

            var quadro = new QuadroTesteBuilder().PreenchidoCom(numeroCaracteres: tamanhoConteudo, caractere: 'a').Construir();
            mockGerenciadorDisco.Setup(a => a.CarregarPagina(IndiceZero)).Returns(quadro.Pagina);
            gerenciadorBuffer.InicializarBuffer();

            gerenciadorBuffer.AtualizarPagina(IndiceZero, ConteudoPaginaTesteHelper.NovoConteudo(tamanhoConteudo, 'x'));

            var paginaRecuperada = buffer.Obter(IndiceZero);
            paginaRecuperada.Sujo.Should().BeTrue("deve marcar uma página como suja ao atualizar seu conteúdo");
            APaginaDeveConterApenas(paginaRecuperada, 'x');
        }
        
        [Fact]
        public void lendo_uma_pagina_que_ja_esta_no_buffer()
        {
            var quadro = new QuadroTesteBuilder()
                .NoIndice(IndiceUm).ComPinCount(0)
                .Construir();

            var mockGerenciadorDisco = new Mock<IGerenciadorEspacoEmDisco>();
            var mockBuffer = new Mock<IPoolDeBuffers>();
            mockBuffer.Setup(buffer => buffer.Obter(IndiceUm)).Returns(quadro);

            var gerenciadorBuffer = new GerenciadorBuffer(mockGerenciadorDisco.Object, mockBuffer.Object, UmaConfiguracaoDeBuffer(1));
            var quadroRecuperado = gerenciadorBuffer.LerPagina(IndiceUm);

            quadroRecuperado.IndicePaginaNoDisco.Should().Be(quadro.IndicePaginaNoDisco);
            mockBuffer.Verify(b => b.Obter(IndiceUm), Times.Once);
            mockGerenciadorDisco.Verify(b => b.CarregarPagina(IndiceUm), Times.Never);

            DeveIncrementarOPinCount(quadro:quadroRecuperado, pinCountAnterior:0);
        }

        private void DeveIncrementarOPinCount(IQuadro quadro, int pinCountAnterior)
        {
            quadro.PinCount.Should().Be(pinCountAnterior + 1, "o pincount deve ser incrementado ao ler uma pagina que ja esta no buffer");
        }

        [Fact]
        public void lendo_uma_pagina_que_ainda_nao_esta_no_buffer()
        {
            var quadroNoDisco = new QuadroTesteBuilder().NoIndice(IndiceUm).Construir();

            var mockGerenciadorDisco = new Mock<IGerenciadorEspacoEmDisco>();
            mockGerenciadorDisco.Setup(a => a.CarregarPagina(IndiceUm)).Returns(quadroNoDisco.Pagina);

            var mockBuffer = new Mock<IPoolDeBuffers>();
            var gerenciadorBuffer = new GerenciadorBuffer(mockGerenciadorDisco.Object, mockBuffer.Object, UmaConfiguracaoDeBuffer(1));
            var paginaRecuperada = gerenciadorBuffer.LerPagina(IndiceUm);

            var sequencia = new MockSequence();
            mockBuffer.InSequence(sequencia).Setup(b => b.Obter(IndiceUm));
            mockGerenciadorDisco.InSequence(sequencia).Setup(b => b.CarregarPagina(IndiceUm));
            mockBuffer.InSequence(sequencia).Setup(b => b.Armazenar(It.Is<IQuadro>(p => p.IndicePaginaNoDisco == IndiceUm)));
        }

        [Fact]
        public void listando_as_paginas_no_buffer()
        {
            var mockBuffer = new Mock<IPoolDeBuffers>();
            mockBuffer.Setup(b => b.ListarPaginas()).Returns(new IResumoPagina[2]);

            var gerenciadorBuffer = new GerenciadorBuffer(new Mock<IGerenciadorEspacoEmDisco>().Object, mockBuffer.Object, UmaConfiguracaoDeBuffer(3));
            IEnumerable<IResumoPagina> resumoBuffer = gerenciadorBuffer.ListarPaginas();

            mockBuffer.Verify(b => b.ListarPaginas());
            resumoBuffer.Should().HaveCount(2, "deveria listar os três itens do buffer");
        }

        [Fact]
        public void inicializando_o_buffer()
        {
            const int numeroPaginasNoCarregadas = 10;

            var mockGerenciadorDisco = new Mock<IGerenciadorEspacoEmDisco>();
            for (int i = 0; i < numeroPaginasNoCarregadas; i++)
            {
                mockGerenciadorDisco.Setup(a => a.CarregarPagina(i)).Returns(new QuadroTesteBuilder().Construir().Pagina);
            }

            var mockBuffer = new Mock<IPoolDeBuffers>();
            var gerenciadorBuffer = new GerenciadorBuffer(mockGerenciadorDisco.Object, mockBuffer.Object, UmaConfiguracaoDeBuffer(numeroPaginasNoCarregadas));
            gerenciadorBuffer.InicializarBuffer();

            mockBuffer.Verify(buffer => buffer.Armazenar(It.IsAny<IQuadro>()), Times.Exactly(numeroPaginasNoCarregadas));
        }

        private static void DadoQueExisteUmaPaginaEmDiscoNoIndice(Mock<IGerenciadorEspacoEmDisco> mockGerenciadorDisco, int indicePagina)
        {
            mockGerenciadorDisco.Setup(buffer => buffer.CarregarPagina(indicePagina))
                .Returns(new QuadroTesteBuilder().NoIndice(indicePagina).Construir().Pagina);
        }

        private IConfiguracaoBuffer UmaConfiguracaoDeBuffer(int limiteDePaginasEmMemoria)
        {
            return new ConfiguracaoBuffer
            {
                LimiteDePaginasEmMemoria = limiteDePaginasEmMemoria
            };
        }

        public void APaginaDeveConterApenas(IQuadro pagina, char caractere)
        {
            foreach (var c in pagina.Pagina.Conteudo)
            {
                c.Should().Be(caractere);
            }
        }
    }
}