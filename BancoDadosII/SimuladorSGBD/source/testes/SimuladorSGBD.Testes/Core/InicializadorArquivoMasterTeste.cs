using Moq;
using SimuladorSGBD.Core;
using System;
using System.IO;
using SimuladorSGBD.Core.IO;
using Xunit;

namespace SimuladorSGBD.Testes.Core
{
    public class InicializadorArquivoMasterTeste
    {
        private readonly string arquivoMaster = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "arquivoMaster.txt");

        public InicializadorArquivoMasterTeste()
        {
            var arquivo = new FileInfo(arquivoMaster);
            if(arquivo.Exists)
                arquivo.Delete();
        }

        [Fact]
        public void criacao_do_arquivo_principal_do_banco_quando_nao_existe()
        {
            var mockManipuladorArquivoFactory = new Mock<IManipuladorArquivoMestreFactory>();
            var mockArquivoMestre = new Mock<IArquivoMestre>();
            mockManipuladorArquivoFactory.Setup(m => m.Criar()).Returns(mockArquivoMestre.Object);

            var inicializadorArquivoMaster = new InicializadorArquivoMaster(mockManipuladorArquivoFactory.Object);
            inicializadorArquivoMaster.Inicializar(arquivoMaster);

            mockArquivoMestre.Verify(m => m.CriarArquivoSeNaoExiste(It.IsAny<int>(), It.IsAny<int>()), Times.Once, "deveria ter criado o arquivo principal");
        }

        [Fact]
        public void o_arquivo_principal_criado_deve_ter_20_blocos_de_128_bytes()
        {
            var mockManipuladorArquivoFactory = new Mock<IManipuladorArquivoMestreFactory>();
            var mockArquivoMestre = new Mock<IArquivoMestre>();
            mockManipuladorArquivoFactory.Setup(m => m.Criar()).Returns(mockArquivoMestre.Object);
            mockArquivoMestre.SetupGet(m => m.ExisteNoDisco).Returns(false);

            var inicializadorArquivoMaster = new InicializadorArquivoMaster(mockManipuladorArquivoFactory.Object);
            inicializadorArquivoMaster.Inicializar(arquivoMaster);

            mockArquivoMestre.Verify(m => m.CriarArquivoSeNaoExiste(20, 128), Times.Once, "deveria ter criado 20 blocos de 128 bytes");
        }
    }
}