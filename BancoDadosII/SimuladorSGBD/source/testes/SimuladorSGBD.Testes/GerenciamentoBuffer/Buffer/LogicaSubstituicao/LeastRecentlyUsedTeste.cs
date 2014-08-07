using FluentAssertions;
using SimuladorSGBD.Core.GerenciamentoBuffer.Buffer.LogicaSubstituicao;
using Xunit;

namespace SimuladorSGBD.Testes.GerenciamentoBuffer.Buffer.LogicaSubstituicao
{
    public class LeastRecentlyUsedTeste
    {
        [Fact]
        public void buscando_um_quadro_com_pin_count_zero_quando_o_buffer_esta_vazio()
        {
            var lru = new LeastRecentlyUsed();
            int indiceSelecionado = lru.Selecionar();

            indiceSelecionado.Should().Be(-1);
        }

        [Fact]
        public void buscando_um_quadro_com_pin_count_zero_com_um_item_no_buffer()
        {
            var lru = new LeastRecentlyUsed();
            lru.NotificarDecrementoPinCount(indice: 5, novoPinCount:0);

            int indiceSelecionado = lru.Selecionar();

            indiceSelecionado.Should().Be(5);
        }

        [Fact]
        public void buscando_o_quadro_mais_antigo_de_pin_count_zero()
        {
            var lru = new LeastRecentlyUsed();
            lru.NotificarDecrementoPinCount(indice: 5, novoPinCount: 0);
            lru.NotificarDecrementoPinCount(indice: 2, novoPinCount: 0);
            lru.NotificarDecrementoPinCount(indice: 10, novoPinCount: 0);
            
            int indiceSelecionado = lru.Selecionar();

            indiceSelecionado.Should().Be(5);
        }

        [Fact]
        public void ignorando_quadros_com_pin_count_maior_que_zero()
        {
            var lru = new LeastRecentlyUsed();
            lru.NotificarDecrementoPinCount(indice: 5, novoPinCount: 1);
            lru.NotificarDecrementoPinCount(indice: 2, novoPinCount: 0);
            lru.NotificarDecrementoPinCount(indice: 10, novoPinCount: 2);
            lru.NotificarDecrementoPinCount(indice: 7, novoPinCount: 0);
            
            int primeiroIindiceSelecionado = lru.Selecionar();
            int segundoIndiceSelecionado = lru.Selecionar();

            primeiroIindiceSelecionado.Should().Be(2);
            segundoIndiceSelecionado.Should().Be(7);
        }
    }
}