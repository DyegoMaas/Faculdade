using FluentAssertions;
using SimuladorSGBD.Core.GerenciamentoBuffer;
using SimuladorSGBD.Testes.Fixtures;
using Xunit;
using Xunit.Extensions;

namespace SimuladorSGBD.Testes.GerenciamentoBuffer
{
    public class BufferEmMemoriaTeste
    {
        [Fact]
        public void armazenando_uma_pagina_no_buffer_e_recuperando_a_pagina()
        {
            var paginaArmazenada = new PaginaTesteBuilder().Construir();

            IBufferEmMemoria buffer = new BufferEmMemoria();
            buffer.Armazenar(paginaArmazenada);

            var paginaRecuperada = buffer.Obter(paginaArmazenada.IndicePaginaNoDisco);
            paginaRecuperada.Should().Be(paginaArmazenada);
        }

        [Fact]
        public void sobrescrevendo_paginas_de_mesmo_indice_no_buffer()
        {
            const int indiceUm = 1;

            var paginaOriginal = new PaginaTesteBuilder().NoIndice(indiceUm).Construir();
            var novaPagina = new PaginaTesteBuilder().NoIndice(indiceUm).Construir();

            var buffer = new BufferEmMemoria();
            buffer.Armazenar(paginaOriginal);
            buffer.Armazenar(novaPagina);

            var paginaRecuperada = buffer.Obter(indiceUm);
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
                buffer.Armazenar(new PaginaTesteBuilder().NoIndice(indice).Construir());
            }

            buffer.NumeroPaginasNoBuffer.Should().Be(numeroPaginasNoBuffer);
        }
        
        private static BufferEmMemoria DadoUmBufferVazio()
        {
            return new BufferEmMemoria();
        }
    }
}