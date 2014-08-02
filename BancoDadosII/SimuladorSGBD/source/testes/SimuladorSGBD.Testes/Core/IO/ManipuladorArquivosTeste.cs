using FluentAssertions;
using SimuladorSGBD.Core.IO;
using Xunit;

namespace SimuladorSGBD.Testes.Core.IO
{
    public class ManipuladorArquivosTeste
    {
        [Fact]
        public void criacao_de_um_manipulador_de_arquivo_mestre()
        {
            var manipuladorArquivos = new ManipuladorArquivos();
            IManipuladorArquivo manipuladorArquivo = manipuladorArquivos.Manipular("um arquivo");
            manipuladorArquivo.Should().NotBeNull();
        }
    }
}