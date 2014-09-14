using Moq;
using SimuladorSGBD.Core;
using SimuladorSGBD.Core.IO;
using Xunit;

namespace SimuladorSGBD.Testes.Core
{
    public class InicializadorArquivoHeapTeste
    {
        [Fact]
        public void criacao_do_arquivo_principal_do_banco_quando_nao_existe()
        {
            var mockGerenciadorEspacoEmDisco = new Mock<IGerenciadorEspacoEmDisco>();

            var inicializadorArquivoMaster = new InicializadorArquivoHeap(mockGerenciadorEspacoEmDisco.Object);
            inicializadorArquivoMaster.Inicializar(blocos:20, bytes:128);

            mockGerenciadorEspacoEmDisco.Verify(m => m.CriarArquivoSeNaoExiste(It.IsAny<int>(), It.IsAny<int>()), Times.Once, "deveria ter criado o arquivo principal");
        }

        [Fact]
        public void o_arquivo_principal_criado_deve_ter_20_blocos_de_128_bytes()
        {
            var mockGerenciadorEspacoEmDisco = new Mock<IGerenciadorEspacoEmDisco>();
            mockGerenciadorEspacoEmDisco.SetupGet(m => m.ExisteNoDisco).Returns(false);

            var inicializadorArquivoMaster = new InicializadorArquivoHeap(mockGerenciadorEspacoEmDisco.Object);
            inicializadorArquivoMaster.Inicializar(blocos: 20, bytes: 128);

            mockGerenciadorEspacoEmDisco.Verify(m => m.CriarArquivoSeNaoExiste(20, 128), Times.Once, "deveria ter criado 20 blocos de 128 bytes");
        }
    }
}