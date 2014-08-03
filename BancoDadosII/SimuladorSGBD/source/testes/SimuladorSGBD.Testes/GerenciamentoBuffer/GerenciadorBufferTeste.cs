using FluentAssertions;
using Moq;
using SimuladorSGBD.Core;
using SimuladorSGBD.Core.GerenciamentoBuffer;
using SimuladorSGBD.Core.IO;
using SimuladorSGBD.Testes.Fixtures;
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

            var gerenciadorBuffer = new GerenciadorBuffer(mockArquivoMestre.Object);
            IPaginaEmMemoria pagina = gerenciadorBuffer.CarregarPagina(indicePagina);
            pagina.Dados.Should().HaveSameCount(paginaNoDisco.Dados);
            pagina.Sujo.Should().Be(false);
            pagina.PinCount.Should().Be(0);
            pagina.UltimoAcesso.Should().Be(0);

            mockArquivoMestre.Verify(m => m.CarregarPagina(indicePagina));
        }
    }
}