using FluentAssertions;
using SimuladorSGBD.Core.IO;
using SimuladorSGBD.Testes.Fixtures;
using Xunit;

namespace SimuladorSGBD.Testes.Core.IO
{
    public class ManipuladorArquivoMestreFactoryTeste
    {
        [Fact]
        public void criacao_de_um_manipulador_de_arquivo_mestre()
        {
            var manipuladorArquivos = new ManipuladorArquivoMestreFactory(new ConfiguracaoIOFake());
            IArquivoMestre arquivoMestre = manipuladorArquivos.Criar();
            arquivoMestre.Should().NotBeNull();
        }
    }
}