using System.Linq;
using FluentAssertions;
using SimuladorSGBD.Core.GerenciamentoBuffer.Buffer;
using SimuladorSGBD.Testes.Fixtures;
using Xunit;
using Xunit.Extensions;

namespace SimuladorSGBD.Testes.Core.GerenciamentoBuffer.Buffer
{
    public class PoolDeBuffersTeste
    {
        private const int IndiceZero = 0;
        private const int IndiceUm = 1;
        private const int IndiceDois = 2;

        [Fact]
        public void armazenando_uma_pagina_no_buffer_e_recuperando_a_pagina()
        {
            var quadroArmazenado = new QuadroTesteBuilder().Construir();

            IPoolDeBuffers buffer = new PoolDeBuffers();
            buffer.Armazenar(quadroArmazenado);

            var quadroRecuperado = buffer.Obter(quadroArmazenado.IndicePaginaNoDisco);
            quadroRecuperado.Should().Be(quadroArmazenado);
        }

        [Fact]
        public void sobrescrevendo_paginas_de_mesmo_indice_no_buffer()
        {
            var quadroOriginal = new QuadroTesteBuilder().NoIndice(IndiceUm).Construir();
            var novoQuadro = new QuadroTesteBuilder().NoIndice(IndiceUm).Construir();

            var buffer = new PoolDeBuffers();
            buffer.Armazenar(quadroOriginal);
            buffer.Armazenar(novoQuadro);

            var paginaRecuperada = buffer.Obter(IndiceUm);
            paginaRecuperada.Should().Be(novoQuadro);
        }

        [Fact]
        public void retorna_nulo_ao_obter_uma_pagina_inexistente()
        {
            var buffer = DadoUmBufferVazio();
            var quadroRecuperado = buffer.Obter(0);

            quadroRecuperado.Should().BeNull();
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
            var quadrosNoBuffer = new[]
            {
                new QuadroTesteBuilder().NoIndice(IndiceZero).Construir(),
                new QuadroTesteBuilder().NoIndice(IndiceUm).Sujo().Construir(),
                new QuadroTesteBuilder().NoIndice(IndiceDois).Sujo().ComPinCount(2).Construir()
            }.ToList();

            var buffer = new PoolDeBuffers();
            quadrosNoBuffer.ForEach(buffer.Armazenar);

            var resumosQuadros = buffer.ListarQuadros().ToArray();
            resumosQuadros.Should().HaveSameCount(quadrosNoBuffer);

            for (var i = 0; i < resumosQuadros.Length; i++)
            {
                var resumo = resumosQuadros[i];
                var quadroNoBuffer = quadrosNoBuffer[i];

                resumo.Conteudo.Should().BeEquivalentTo(quadroNoBuffer.Pagina.Conteudo);
                resumo.IndiceNoDisco.Should().Be(quadroNoBuffer.IndicePaginaNoDisco);
                resumo.PinCount.Should().Be(quadroNoBuffer.PinCount);
                resumo.Sujo.Should().Be(quadroNoBuffer.Sujo);
            }
        }

        [Fact]
        public void excluindo_um_quadro_do_buffer()
        {
            var buffer = new PoolDeBuffers();

            var quadro = new QuadroTesteBuilder().NoIndice(IndiceUm).Construir();
            buffer.Armazenar(quadro);
            buffer.Remover(IndiceUm);
            buffer.Obter(IndiceUm).Should().BeNull("deveria ter excluido o quadro");
        }
        
        private static PoolDeBuffers DadoUmBufferVazio()
        {
            return new PoolDeBuffers();
        }
    }
}