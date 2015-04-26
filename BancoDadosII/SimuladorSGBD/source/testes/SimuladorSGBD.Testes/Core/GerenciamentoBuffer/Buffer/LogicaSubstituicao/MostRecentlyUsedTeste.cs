using FluentAssertions;
using SimuladorSGBD.Core.GerenciamentoBuffer.Buffer.LogicaSubstituicao;
using Xunit;

namespace SimuladorSGBD.Testes.Core.GerenciamentoBuffer.Buffer.LogicaSubstituicao
{
    public class MostRecentlyUsedTeste
    {
        [Fact]
        public void buscando_um_quadro_com_pin_count_zero_quando_o_buffer_esta_vazio()
        {
            var mru = new MostRecentlyUsed();
            var indiceSelecionado = mru.Selecionar();

            indiceSelecionado.Should().Be(-1);
        }

        [Fact]
        public void buscando_um_quadro_com_pin_count_zero_com_um_item_no_buffer()
        {
            var mru = new MostRecentlyUsed();
            mru.NotificarDecrementoPinCount(indice: 5, novoPinCount:0);

            int indiceSelecionado = mru.Selecionar();

            indiceSelecionado.Should().Be(5);
        }

        [Fact]
        public void buscando_o_quadro_mais_antigo_de_pin_count_zero()
        {
            var mru = new MostRecentlyUsed();
            mru.NotificarDecrementoPinCount(indice: 5, novoPinCount: 0);
            mru.NotificarDecrementoPinCount(indice: 2, novoPinCount: 0);
            mru.NotificarDecrementoPinCount(indice: 10, novoPinCount: 0);
            
            int indiceSelecionado = mru.Selecionar();

            indiceSelecionado.Should().Be(10);
        }

        [Fact]
        public void ignorando_quadros_com_pin_count_maior_que_zero()
        {
            var mru = new MostRecentlyUsed();
            mru.NotificarDecrementoPinCount(indice: 5, novoPinCount: 1);
            mru.NotificarDecrementoPinCount(indice: 2, novoPinCount: 0);
            mru.NotificarDecrementoPinCount(indice: 10, novoPinCount: 2);
            mru.NotificarDecrementoPinCount(indice: 7, novoPinCount: 0);
            
            int primeiroIindiceSelecionado = mru.Selecionar();
            int segundoIndiceSelecionado = mru.Selecionar();

            primeiroIindiceSelecionado.Should().Be(7);
            segundoIndiceSelecionado.Should().Be(2);
        }
    }
}