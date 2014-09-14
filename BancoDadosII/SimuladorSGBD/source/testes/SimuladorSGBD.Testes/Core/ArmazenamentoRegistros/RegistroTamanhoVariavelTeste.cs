using FluentAssertions;
using Xunit;

namespace SimuladorSGBD.Testes.Core.ArmazenamentoRegistros
{
    public class RegistroTamanhoVariavelTeste
    {
        [Fact]
        public void criando_um_registro_sem_campos()
        {
            var registro = new RegistroTamanhoVariavel();
            registro.NumeroCampos.Should().Be(0);
        }

        [Fact]
        public void criando_um_registro_com_dois_campos()
        {
            var registro = new RegistroTamanhoVariavel();
            registro.AdicionarCampo("conteúdo");
            registro.AdicionarCampo("12545abcz");
            
            registro.NumeroCampos.Should().Be(2);

            //TODO continuar
        }
    }

    public class RegistroTamanhoVariavel
    {
        public int NumeroCampos { get; set; }

        public void AdicionarCampo(string conteudo)
        {
            throw new System.NotImplementedException();
        }
    }
}