using FluentAssertions;
using SimuladorSGBD.Core.GerenciamentoBuffer;
using SimuladorSGBD.Core.GerenciamentoBuffer.Buffer.LogicaSubstituicao;
using Xunit;

namespace SimuladorSGBD.Testes.Core.GerenciamentoBuffer.Buffer.LogicaSubstituicao
{
    public class LogicaSubstituicaoFactoryTeste
    {
        [Fact]
        public void requisitando_um_lru()
        {
            var factory = new LogicaSubstituicaoFactory();
            ILogicaSubstituicao lru = factory.LRU();

            lru.Should().NotBeNull();
            lru.Should().BeOfType<LeastRecentlyUsed>();
        }

        [Fact]
        public void requisitando_um_mru()
        {
            var factory = new LogicaSubstituicaoFactory();
            ILogicaSubstituicao mru = factory.MRU();

            mru.Should().NotBeNull();
            mru.Should().BeOfType<MostRecentlyUsed>();
        }
    }
}