using FluentAssertions;
using SimuladorSGBD.Core.IO;
using SimuladorSGBD.Testes.Fixtures;
using Xunit;

namespace SimuladorSGBD.Testes.Core.IO
{
    public class ManipuladorArquivosTeste
    {
        [Fact]
        public void criacao_de_um_manipulador_de_arquivo_mestre()
        {
            var manipuladorArquivos = new ManipuladorArquivoMestreFactoryMestreFactory(new ConfiguracaoIOFake());
            IManipuladorArquivoMestre manipuladorArquivoMestre = manipuladorArquivos.Criar();
            manipuladorArquivoMestre.Should().NotBeNull();
        }
    }
}