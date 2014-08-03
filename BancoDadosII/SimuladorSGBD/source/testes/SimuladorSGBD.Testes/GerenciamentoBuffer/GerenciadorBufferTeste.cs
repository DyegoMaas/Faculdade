using FluentAssertions;
using Moq;
using SimuladorSGBD.Core;
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

            var paginaNoDisco = new PaginaFake
            {
                Dados = new char[128],
                Sujo = true,
                PinCount = 1,
                UltimoAcesso = 12
            };
            mockArquivoMestre.Setup(m => m.CarregarPagina(indicePagina)).Returns(paginaNoDisco);

            var gerenciadorBuffer = new GerenciadorBuffer(mockArquivoMestre.Object);
            IPagina pagina = gerenciadorBuffer.CarregarPagina(indicePagina);
            pagina.Dados.Should().HaveSameCount(paginaNoDisco.Dados);
            pagina.Sujo.Should().Be(paginaNoDisco.Sujo);
            pagina.PinCount.Should().Be(paginaNoDisco.PinCount);
            pagina.UltimoAcesso.Should().Be(paginaNoDisco.UltimoAcesso);

            mockArquivoMestre.Verify(m => m.CarregarPagina(indicePagina));
        }
    }

    //TODO: mover para o projeto Core
    public class GerenciadorBuffer
    {
        private readonly IArquivoMestre arquivoMestre;

        public GerenciadorBuffer(IArquivoMestre arquivoMestre)
        {
            this.arquivoMestre = arquivoMestre;
        }

        public IPagina CarregarPagina(int indice)
        {
            return arquivoMestre.CarregarPagina(indice);
        }
        
        public void SalvarPagina(int indice, IPagina pagina)
        {
            throw new System.NotImplementedException();
        }
    }
}