using System.Linq;
using FluentAssertions;
using SimuladorSGBD.Core.GerenciamentoBuffer.Buffer;
using SimuladorSGBD.Testes.Fixtures;
using Xunit;
using Xunit.Extensions;

namespace SimuladorSGBD.Testes.GerenciamentoBuffer.Buffer
{
    public class BufferEmMemoriaTeste
    {
        private const int IndiceZero = 0;
        private const int IndiceUm = 1;
        private const int IndiceDois = 2;

        [Fact]
        public void armazenando_uma_pagina_no_buffer_e_recuperando_a_pagina()
        {
            var paginaArmazenada = new QuadroTesteBuilder().Construir();

            IPoolDeBuffers buffer = new PoolDeBuffers();
            buffer.Armazenar(paginaArmazenada);

            var paginaRecuperada = buffer.Obter(paginaArmazenada.IndicePaginaNoDisco);
            paginaRecuperada.Should().Be(paginaArmazenada);
        }

        [Fact]
        public void sobrescrevendo_paginas_de_mesmo_indice_no_buffer()
        {
            var paginaOriginal = new QuadroTesteBuilder().NoIndice(IndiceUm).Construir();
            var novaPagina = new QuadroTesteBuilder().NoIndice(IndiceUm).Construir();

            var buffer = new PoolDeBuffers();
            buffer.Armazenar(paginaOriginal);
            buffer.Armazenar(novaPagina);

            var paginaRecuperada = buffer.Obter(IndiceUm);
            paginaRecuperada.Should().Be(novaPagina);
        }

        [Fact]
        public void retorna_nulo_ao_obter_uma_pagina_inexistente()
        {
            var buffer = DadoUmBufferVazio();
            var paginaRecuperada = buffer.Obter(0);

            paginaRecuperada.Should().BeNull();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(5)]
        public void identificando_o_numero_de_paginas_no_buffer(int numeroPaginasNoBuffer)
        {
            var buffer = DadoUmBufferVazio();

            for (int indice = 0; indice < numeroPaginasNoBuffer; indice++)
            {
                buffer.Armazenar(new QuadroTesteBuilder().NoIndice(indice).Construir());
            }

            buffer.NumeroPaginasNoBuffer.Should().Be(numeroPaginasNoBuffer);
        }

        [Fact]
        public void listando_as_paginas_no_buffer()
        {
            var paginasNoBuffer = new[]
            {
                new QuadroTesteBuilder().NoIndice(IndiceZero).Construir(),
                new QuadroTesteBuilder().NoIndice(IndiceUm).Sujo().Construir(),
                new QuadroTesteBuilder().NoIndice(IndiceDois).Sujo().ComPinCount(2).Construir()
            }.ToList();

            var buffer = new PoolDeBuffers();
            paginasNoBuffer.ForEach(buffer.Armazenar);

            var resumosPaginas = buffer.ListarPaginas().ToArray();
            resumosPaginas.Should().HaveSameCount(paginasNoBuffer);

            for (var i = 0; i < resumosPaginas.Length; i++)
            {
                var resumo = resumosPaginas[i];
                var paginaNoBuffer = paginasNoBuffer[i];

                resumo.Conteudo.Should().BeSameAs(paginaNoBuffer.Pagina.Conteudo);
                resumo.IndiceNoDisco.Should().Be(paginaNoBuffer.IndicePaginaNoDisco);
                resumo.PinCount.Should().Be(paginaNoBuffer.PinCount);
                resumo.Sujo.Should().Be(paginaNoBuffer.Sujo);
            }
        }
        
        private static PoolDeBuffers DadoUmBufferVazio()
        {
            return new PoolDeBuffers();
        }
    }
}