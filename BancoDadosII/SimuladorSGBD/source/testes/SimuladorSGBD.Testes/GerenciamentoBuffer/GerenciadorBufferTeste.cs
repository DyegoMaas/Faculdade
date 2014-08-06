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

        readonly Mock<IGerenciadorEspacoEmDisco> mockGerenciadorDisco = new Mock<IGerenciadorEspacoEmDisco>();
        readonly Mock<IPoolDeBuffers> mockBuffer = new Mock<IPoolDeBuffers>();
        readonly Mock<ILogicaSubstituicao> mockLogicaSubstituicao = new Mock<ILogicaSubstituicao>();
        
        [Theory,
        InlineData(1),
        InlineData(2),
        InlineData(10)]
        public void carregando_uma_pagina_para_o_buffer(int indicePagina)
        {
            var quadroDisco = new QuadroTesteBuilder().NoIndice(indicePagina).PreenchidoCom(128, 'a').Construir();
            var quadroNoBuffer = new QuadroTesteBuilder().NoIndice(IndiceZero).PreenchidoCom(128, 'x').Construir();
            mockGerenciadorDisco.Setup(m => m.CarregarPagina(indicePagina)).Returns(quadroDisco.Pagina);

            IQuadro quadro = null;
            mockBuffer.Setup(b => b.Armazenar(It.IsAny<IQuadro>())).Callback<IQuadro>(q => quadro = q);
            mockBuffer.Setup(b => b.Obter(IndiceZero)).Returns(quadroNoBuffer);

            var gerenciadorBuffer = new GerenciadorBuffer(mockGerenciadorDisco.Object, mockLogicaSubstituicao.Object, 
                mockBuffer.Object, UmaConfiguracaoDeBuffer(limiteDePaginasEmMemoria: 1));
            gerenciadorBuffer.ObterPagina(indicePagina);

            APaginaDeveConterApenas(quadro, 'a');
            quadro.Pagina.Conteudo.Should().HaveSameCount(quadroDisco.Pagina.Conteudo);
            quadro.Sujo.Should().Be(false);
            quadro.PinCount.Should().Be(0);
            quadro.UltimoAcesso.Should().Be(0);
            quadro.IndicePaginaNoDisco.Should().Be(indicePagina);

            mockGerenciadorDisco.Verify(m => m.CarregarPagina(indicePagina));
        }

        [Fact]
        public void salvando_uma_pagina_no_disco()
        {
            var quadroNoBuffer = new QuadroTesteBuilder().NoIndice(IndiceUm).Construir();
            mockBuffer.Setup(buffer => buffer.Obter(IndiceUm)).Returns(quadroNoBuffer);

            var gerenciadorBuffer = DadoUmGerenciadorBufferCom(limitePaginasNoBuffer: 1);
            gerenciadorBuffer.SalvarPagina(IndiceUm);

            mockBuffer.Verify(b => b.Obter(IndiceUm));
            mockGerenciadorDisco.Verify(a => a.SalvarPagina(IndiceUm, It.Is<IPagina>(p => p == quadroNoBuffer.Pagina)), Times.Once, "deveria salvar a página no disco");
        }
        
        //integração
        [Fact]
        public void alterando_uma_pagina_em_memoria()
        {
            const int tamanhoConteudo = 128;

            var buffer = new PoolDeBuffers();
            var gerenciadorBuffer = new GerenciadorBuffer(mockGerenciadorDisco.Object, mockLogicaSubstituicao.Object, buffer, UmaConfiguracaoDeBuffer(limiteDePaginasEmMemoria: 1));

            var quadro = new QuadroTesteBuilder().NoIndice(IndiceZero).PreenchidoCom(numeroCaracteres: tamanhoConteudo, caractere: 'a').Construir();
            mockGerenciadorDisco.Setup(a => a.CarregarPagina(IndiceZero)).Returns(quadro.Pagina);
            gerenciadorBuffer.ObterPagina(IndiceZero);
            gerenciadorBuffer.AtualizarPagina(IndiceZero, ConteudoPaginaTesteHelper.NovoConteudo(tamanhoConteudo, 'x'));

            var paginaRecuperada = buffer.Obter(IndiceZero);
            paginaRecuperada.Sujo.Should().BeTrue("deve marcar uma página como suja ao atualizar seu conteúdo");
            APaginaDeveConterApenas(paginaRecuperada, 'x');
        }
        
        [Fact]
        public void obtendo_uma_pagina_que_ja_esta_no_buffer()
        {
            var quadro = new QuadroTesteBuilder()
                .NoIndice(IndiceUm).ComPinCount(0)
                .Construir();

            mockBuffer.Setup(buffer => buffer.Obter(IndiceUm)).Returns(quadro);

            var gerenciadorBuffer = DadoUmGerenciadorBufferCom(limitePaginasNoBuffer: 1);
            var quadroRecuperado = gerenciadorBuffer.ObterPagina(IndiceUm);

            quadroRecuperado.IndicePaginaNoDisco.Should().Be(quadro.IndicePaginaNoDisco);
            mockBuffer.Verify(b => b.Obter(IndiceUm), Times.Once);
            mockGerenciadorDisco.Verify(b => b.CarregarPagina(IndiceUm), Times.Never);

            DeveIncrementarOPinCount(quadro:quadroRecuperado, pinCountAnterior:0);
        }

        [Theory,
        InlineData(true),
        InlineData(false)]
        public void liberando_uma_pagina(bool paginaFoiAlterada)
        {
            var quadro = new QuadroTesteBuilder()
                .NoIndice(IndiceUm).ComPinCount(1)
                .Construir();

            mockBuffer.Setup(buffer => buffer.Obter(IndiceUm)).Returns(quadro);
            var gerenciadorBuffer = DadoUmGerenciadorBufferCom(limitePaginasNoBuffer: 1);
            gerenciadorBuffer.LiberarPagina(IndiceUm, paginaFoiAlterada);

            DeveDecrementarOPinCount(quadro: quadro, pinCountAnterior: 1);
            quadro.Sujo.Should().Be(paginaFoiAlterada);
        }

        [Theory,
        InlineData(true),
        InlineData(false)]
        public void obtendo_uma_pagina_que_ainda_nao_esta_no_buffer_quando_o_buffer_esta_cheio(bool quadroSubstituidoEstaSujo)
        {
            var quadroZeroSubstituir = new QuadroTesteBuilder().NoIndice(IndiceZero).Sujo(quadroSubstituidoEstaSujo).ComPinCount(1).Construir();
            var quadroUm = new QuadroTesteBuilder().NoIndice(IndiceUm).Construir();

            mockGerenciadorDisco.Setup(a => a.CarregarPagina(IndiceUm)).Returns(quadroUm.Pagina);
            mockBuffer.Setup(b => b.Obter(IndiceZero)).Returns(quadroZeroSubstituir);
            mockBuffer.SetupGet(b => b.NumeroPaginasNoBuffer).Returns(1);
            mockLogicaSubstituicao.Setup(l => l.Selecionar()).Returns(IndiceZero);

            var gerenciadorBuffer = DadoUmGerenciadorBufferCom(limitePaginasNoBuffer: 1);
            var paginaRecuperada = gerenciadorBuffer.ObterPagina(IndiceUm);

            mockBuffer.Verify(b => b.Obter(IndiceUm), Times.Once);
            mockGerenciadorDisco.Verify(b => b.CarregarPagina(IndiceUm), Times.Once);
            mockLogicaSubstituicao.Verify(l => l.Selecionar(), Times.Once);
            DeveIncrementarOPinCount(quadro: quadroZeroSubstituir, pinCountAnterior: 1);

            if (quadroSubstituidoEstaSujo)
            {
                mockGerenciadorDisco.Verify(b => b.SalvarPagina(IndiceZero, It.IsAny<IPagina>()), Times.Once);
            }
            mockBuffer.Verify(b => b.Remover(IndiceZero), Times.Once);
            mockBuffer.Verify(b => b.Armazenar(It.Is<IQuadro>(p => p.IndicePaginaNoDisco == IndiceUm)), Times.Once);
        }

        [Fact]
        public void obtendo_uma_pagina_que_ainda_nao_esta_no_buffer()
        {
            var quadro = new QuadroTesteBuilder().NoIndice(IndiceUm).ComPinCount(0).Construir();

            mockGerenciadorDisco.Setup(a => a.CarregarPagina(IndiceUm)).Returns(quadro.Pagina);

            var gerenciadorBuffer = DadoUmGerenciadorBufferCom(limitePaginasNoBuffer: 1);
            var paginaRecuperada = gerenciadorBuffer.ObterPagina(IndiceUm);

            mockBuffer.Verify(b => b.Obter(IndiceUm), Times.Once);
            mockGerenciadorDisco.Verify(b => b.CarregarPagina(IndiceUm), Times.Once);
            mockLogicaSubstituicao.Verify(l => l.Selecionar(), Times.Never);
            NaoDeveAlterarOPinCount(quadro, pinCountAnterior: 0);

            mockBuffer.Verify(b => b.Armazenar(It.Is<IQuadro>(p => p.IndicePaginaNoDisco == IndiceUm)), Times.Once);
        }

        [Fact]
        public void listando_as_paginas_no_buffer()
        {
            mockBuffer.Setup(b => b.ListarQuadros()).Returns(new IResumoPagina[2]);

            var gerenciadorBuffer = DadoUmGerenciadorBufferCom(limitePaginasNoBuffer:3);
            IEnumerable<IResumoPagina> resumoBuffer = gerenciadorBuffer.ListarPaginas();

            mockBuffer.Verify(b => b.ListarQuadros());
            resumoBuffer.Should().HaveCount(2, "deveria listar os três itens do buffer");
        }

        private GerenciadorBuffer DadoUmGerenciadorBufferCom(int limitePaginasNoBuffer)
        {
            return new GerenciadorBuffer(mockGerenciadorDisco.Object, mockLogicaSubstituicao.Object, mockBuffer.Object, UmaConfiguracaoDeBuffer(limitePaginasNoBuffer));
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

        private void DeveIncrementarOPinCount(IQuadro quadro, int pinCountAnterior)
        {
            quadro.PinCount.Should().Be(pinCountAnterior + 1, "o pincount deve ser incrementado");
        }

        private void DeveDecrementarOPinCount(IQuadro quadro, int pinCountAnterior)
        {
            quadro.PinCount.Should().Be(pinCountAnterior - 1, "o pincount deve ser decrementado ao liberar uma pagina");
        }

        private void NaoDeveAlterarOPinCount(IQuadro quadro, int pinCountAnterior)
        {
            quadro.PinCount.Should().Be(pinCountAnterior, "o pincount nao deveria ter sido alterado");
        }
    }
}