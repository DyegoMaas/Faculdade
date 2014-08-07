using Moq;
using SimuladorSGBD.Core;
using SimuladorSGBD.Core.IO;
using Xunit;

namespace SimuladorSGBD.Testes.Core
{
    public class InicializadorArquivoMestreTeste
    {
        [Fact]
        public void criacao_do_arquivo_principal_do_banco_quando_nao_existe()
        {
            var mockArquivoMestre = new Mock<IGerenciadorEspacoEmDisco>();

            var inicializadorArquivoMaster = new InicializadorArquivoMestre(mockArquivoMestre.Object);
            inicializadorArquivoMaster.Inicializar(blocos:20, bytes:128);

            mockArquivoMestre.Verify(m => m.CriarArquivoSeNaoExiste(It.IsAny<int>(), It.IsAny<int>()), Times.Once, "deveria ter criado o arquivo principal");
        }

        [Fact]
        public void o_arquivo_principal_criado_deve_ter_20_blocos_de_128_bytes()
        {
            var mockArquivoMestre = new Mock<IGerenciadorEspacoEmDisco>();
            mockArquivoMestre.SetupGet(m => m.ExisteNoDisco).Returns(false);

            var inicializadorArquivoMaster = new InicializadorArquivoMestre(mockArquivoMestre.Object);
            inicializadorArquivoMaster.Inicializar(blocos: 20, bytes: 128);

            mockArquivoMestre.Verify(m => m.CriarArquivoSeNaoExiste(20, 128), Times.Once, "deveria ter criado 20 blocos de 128 bytes");
        }
    }
}