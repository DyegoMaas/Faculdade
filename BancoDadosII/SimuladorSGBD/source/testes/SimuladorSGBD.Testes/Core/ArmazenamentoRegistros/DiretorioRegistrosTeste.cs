using FluentAssertions;
using SimuladorSGBD.Core.ArmazenamentoRegistros;
using Xunit;

namespace SimuladorSGBD.Testes.Core.ArmazenamentoRegistros
{
    public class DiretorioRegistrosTeste
    {
        [Fact]
        public void criando_um_diretorio_de_registros_sem_registros()
        {
            var diretorioRegistros = new DiretorioRegistros();
            diretorioRegistros.EnderecoInicioAreaLivre.Should().Be(0);
            diretorioRegistros.NumeroEntradas.Should().Be(0);
            diretorioRegistros.DiretorioSlots.Should().BeEmpty();
        }

        [Fact]
        public void inserindo_dois_registros()
        {
            const int tamanhoPrimeiroRegistro = 10;
            const int tamanhoSegundoRegistro = 10;

            var diretorioRegistros = new DiretorioRegistros();
            diretorioRegistros.InserirSlot(tamanhoPrimeiroRegistro);
            diretorioRegistros.InserirSlot(tamanhoSegundoRegistro);

            diretorioRegistros.EnderecoInicioAreaLivre.Should().Be(tamanhoPrimeiroRegistro + tamanhoSegundoRegistro - 1);
            diretorioRegistros.NumeroEntradas.Should().Be(2);
            diretorioRegistros.DiretorioSlots.Should().HaveCount(2);

            diretorioRegistros.DiretorioSlots[0].Indice.Should().Be(1);
            diretorioRegistros.DiretorioSlots[0].Tamanho.Should().Be(tamanhoPrimeiroRegistro);

            diretorioRegistros.DiretorioSlots[1].Indice.Should().Be(2);
            diretorioRegistros.DiretorioSlots[1].Tamanho.Should().Be(tamanhoSegundoRegistro);
        }
    }
}