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

            var paginaNoDisco = new PaginaFake { Dados = new char[128] };
            mockArquivoMestre.Setup(m => m.CarregarPagina(indicePagina)).Returns(paginaNoDisco);

            var gerenciadorBuffer = new GerenciadorBuffer(mockArquivoMestre.Object, UmaConfiguracaoDeBuffer(1));
            IPaginaEmMemoria pagina = gerenciadorBuffer.CarregarPagina(indicePagina);
            pagina.Dados.Should().HaveSameCount(paginaNoDisco.Dados);
            pagina.Sujo.Should().Be(false);
            pagina.PinCount.Should().Be(0);
            pagina.UltimoAcesso.Should().Be(0);
            pagina.IndicePaginaNoDisco.Should().Be(indicePagina);

            mockArquivoMestre.Verify(m => m.CarregarPagina(indicePagina));
        }

        [Fact]
        public void salvando_uma_pagina_no_disco()
        {
            const int indiceZero = 0;

            var mockArquivoMestre = new Mock<IArquivoMestre>();
            var gerenciadorBuffer = new GerenciadorBuffer(mockArquivoMestre.Object, UmaConfiguracaoDeBuffer(1));

            var paginaNoDisco = new PaginaFake { Dados = new char[128] };
            mockArquivoMestre.Setup(m => m.CarregarPagina(indiceZero)).Returns(paginaNoDisco);
            gerenciadorBuffer.CarregarPagina(indiceZero);

            gerenciadorBuffer.SalvarPagina(indiceZero);
            mockArquivoMestre.Verify(a => a.SalvarPagina(indiceZero, It.IsAny<IPaginaComDados>()), Times.Once, "deveria salvar a página no disco");
        }

        private IConfiguaracaoBuffer UmaConfiguracaoDeBuffer(int limiteDePaginasEmMemoria)
        {
            return new ConfiguracaoBuffer
            {
                LimiteDePaginasEmMemoria = limiteDePaginasEmMemoria
            };
        }
    }
}