using Moq;
using SimuladorSGBD.Core;
using System;
using System.IO;
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
            var mockManipuladorArquivos = new Mock<IManipuladorArquivos>();
            mockManipuladorArquivos.Setup(m => m.ArquivoExiste(arquivoMaster)).Returns(false);

            var inicializadorArquivoMaster = new InicializadorArquivoMaster(mockManipuladorArquivos.Object);
            inicializadorArquivoMaster.Inicializar(arquivoMaster);

            mockManipuladorArquivos.Verify(m => m.CriarArquivo(arquivoMaster), Times.Once, "deveria ter criado o arquivo principal");
        }

        [Fact]
        public void mantendo_arquivo_principal_do_banco_inalterado_durante_a_inicializado_se_ja_existe()
        {
            var mockManipuladorArquivos = new Mock<IManipuladorArquivos>();
            mockManipuladorArquivos.Setup(m => m.ArquivoExiste(arquivoMaster)).Returns(true);

            var inicializadorArquivoMaster = new InicializadorArquivoMaster(mockManipuladorArquivos.Object);
            inicializadorArquivoMaster.Inicializar(arquivoMaster);

            mockManipuladorArquivos.Verify(m => m.CriarArquivo(arquivoMaster), Times.Never, "não deve alterar o arquivo se já existe");
        }

        [Fact]
        public void o_arquivo_principal_criado_deve_ter_20_blocos_de_128_bytes()
        {
            var mockManipuladorArquivos = new Mock<IManipuladorArquivos>();
            mockManipuladorArquivos.Setup(m => m.ArquivoExiste(arquivoMaster)).Returns(false);

            var inicializadorArquivoMaster = new InicializadorArquivoMaster(mockManipuladorArquivos.Object);
            inicializadorArquivoMaster.Inicializar(arquivoMaster);

            mockManipuladorArquivos.Verify(m => m.CriarBlocoVazio(128), Times.Exactly(20), "deveria ter criado 20 blocos de 128 bytes");
        }
    }
}