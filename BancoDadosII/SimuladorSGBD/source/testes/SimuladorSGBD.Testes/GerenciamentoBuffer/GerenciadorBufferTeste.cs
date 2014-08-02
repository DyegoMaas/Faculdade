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
            var mockManipuladorArquivoMestreFactory = new Mock<IManipuladorArquivoMestreFactory>();
            var mockManipuladorArquivoMestre = new Mock<IManipuladorArquivoMestre>();

            var paginaNoDisco = new PaginaFake
            {
                Dados = new char[128],
                Sujo = true,
                PinCount = 1,
                UltimoAcesso = 12
            };
            mockManipuladorArquivoMestre.Setup(m => m.CarregarPagina(indicePagina)).Returns(paginaNoDisco);

            mockManipuladorArquivoMestreFactory.Setup(m => m.Criar()).Returns(mockManipuladorArquivoMestre.Object);

            var gerenciadorBuffer = new GerenciadorBuffer(mockManipuladorArquivoMestreFactory.Object);
            IPagina pagina = gerenciadorBuffer.CarregarPagina(indicePagina);
            pagina.Dados.Should().HaveSameCount(paginaNoDisco.Dados);
            pagina.Sujo.Should().Be(paginaNoDisco.Sujo);
            pagina.PinCount.Should().Be(paginaNoDisco.PinCount);
            pagina.UltimoAcesso.Should().Be(paginaNoDisco.UltimoAcesso);

            mockManipuladorArquivoMestre.Verify(m => m.CarregarPagina(indicePagina));
        }
    }

    public class GerenciadorBuffer
    {
        private readonly IManipuladorArquivoMestreFactory manipuladorArquivoMestreFactory;

        public GerenciadorBuffer(IManipuladorArquivoMestreFactory manipuladorArquivoMestreFactory)
        {
            this.manipuladorArquivoMestreFactory = manipuladorArquivoMestreFactory;
        }

        public IPagina CarregarPagina(int indice)
        {
            using (var arquivo = manipuladorArquivoMestreFactory.Criar())
            {
                return arquivo.CarregarPagina(indice);
            }
        }
        
        public void SalvarPagina(int indice, IPagina pagina)
        {
            throw new System.NotImplementedException();
        }
    }
}