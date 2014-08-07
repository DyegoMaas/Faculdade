using FluentAssertions;
using SimuladorSGBD.Core.GerenciamentoBuffer.Buffer.LogicaSubstituicao;
using Xunit;

namespace SimuladorSGBD.Testes.GerenciamentoBuffer.Buffer.LogicaSubstituicao
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

        //TODO escrever o resto dos testes
    }
}