using System.Linq;
using FluentAssertions;
using Moq;
using SimuladorSGBD.Core.GerenciamentoBuffer;
using SimuladorSGBD.Testes.Fixtures;
using System;
using System.Text;
using Xunit;

namespace SimuladorSGBD.Testes.Core.ArmazenamentoRegistros
{
    public class ArmazenadorRegistrosTeste
    {
        [Fact]
        public void cadastrando_um_valor_vazio()
        {
            var quadroOriginal = new QuadroTesteBuilder().Construir();
            char[] conteudoEsperado = Enumerable.Repeat(CaratereConteudo, TamanhoConteudo).ToArray();

            var mockGerenciadorBuffer = new Mock<IGerenciadorBuffer>();
            mockGerenciadorBuffer.Setup(gb => gb.ObterQuadro(It.IsAny<int>()))
                .Returns(quadroOriginal);

            char[] arrayEditado = null;
            mockGerenciadorBuffer.Setup(gb => gb.AtualizarPagina(It.IsAny<int>(), It.IsAny<byte[]>()))
                .Callback<int, char[]>((i, array) => arrayEditado = array);

            var armazenadorRegistros = new ArmazenadorRegistros(mockGerenciadorBuffer.Object);
            armazenadorRegistros.Inserir(new Registro { Conteudo = conteudoEsperado });

            mockGerenciadorBuffer.Verify(gb => gb.ObterQuadro(0), Times.Once());
            mockGerenciadorBuffer.Verify(gb => gb.AtualizarPagina(0, It.IsAny<byte[]>()), Times.Once());
            mockGerenciadorBuffer.Verify(gb => gb.LiberarPagina(0, true), Times.Once());

            arrayEditado[0].Should().Be(CaractereCoringa);
            arrayEditado.Skip(1).Take(TamanhoConteudo).Should().ContainInOrder(conteudoEsperado);

            //TODO validar ponteiro
        }

        private bool ValidarInicio(char[] array, char[] conteudoEsperado)
        {
            return array[0] == CaractereCoringa
                && array[1] == CaratereConteudo;
        }

        public const char CaractereCoringa = 'A'; //TODO escolher o wildcard
        private const char CaratereConteudo = 'a';
        private const int TamanhoConteudo = 10;

        private bool ValidarFinal(char[] array)
        {
            var conteudoQuadro = new string(array);

            var ponteiro = new PonteiroRegistro()
            {
                Indice = int.Parse(conteudoQuadro.Substring(conteudoQuadro.Length - 1 - 4)),
                Tamanho = int.Parse(conteudoQuadro.Substring(conteudoQuadro.Length - 1 - 8))
            };

            return ponteiro.Indice == 0 && ponteiro.Tamanho == 0;
        }
    }

    [Serializable]
    public class PonteiroRegistro
    {
        public int Indice { get; set; }
        public int Tamanho { get; set; }
    }

    public class Registro
    {
        public char[] Conteudo { get; set; }
    }

    public class ArmazenadorRegistros
    {
        private readonly IGerenciadorBuffer gerenciadorBuffer;

        public ArmazenadorRegistros(IGerenciadorBuffer gerenciadorBuffer)
        {
            this.gerenciadorBuffer = gerenciadorBuffer;
        }

        public void Inserir(Registro registro)
        {
            var quadro = gerenciadorBuffer.ObterQuadro(0);

            var copiaConteudoQuadro = quadro.Pagina.Conteudo.Clone() as byte[];
            AdicionarRegistroNoQuadro(ref copiaConteudoQuadro, registro);
            gerenciadorBuffer.AtualizarPagina(0, copiaConteudoQuadro);
            gerenciadorBuffer.LiberarPagina(0, true);
        }

        private void AdicionarRegistroNoQuadro(ref byte[] conteudoQuadro, Registro registro)
        {
            //conteudo
            var sourceArray = registro.Conteudo;

            //ponteiro
            var ponteiro = CriarPonteiro(registro);
            AdicionarPonteiroNoQuadro(ref conteudoQuadro, ponteiro);
        }

        private PonteiroRegistro CriarPonteiro(Registro registro)
        {
            return new PonteiroRegistro
            {
                Indice = 0,
                Tamanho = registro.Conteudo.Length
            };
        }

        private void AdicionarPonteiroNoQuadro(ref byte[] alvo, PonteiroRegistro ponteiro, int indice = 0)
        {
            const int tamanhoPonteiro = 8;
            int offset = indice * tamanhoPonteiro;
            
            var bytesIndice = Converter(ponteiro.Indice);
            var charsIndice = Encoding.UTF8.GetChars(bytesIndice, 0, bytesIndice.Length);
            offset += charsIndice.Length;
            Array.Copy(charsIndice, 0, alvo, alvo.Length - 1 - offset, charsIndice.Length);

            var bytesTamanho = Converter(ponteiro.Tamanho);
            var charsTamanho= Encoding.UTF8.GetChars(bytesTamanho, 0, bytesTamanho.Length);
            offset += charsTamanho.Length;
            Array.Copy(charsTamanho, 0, alvo, alvo.Length - 1 - offset, charsTamanho.Length);
        }

        private byte[] Converter(int valor)
        {
            var array = BitConverter.GetBytes(valor);
            if(BitConverter.IsLittleEndian)
                Array.Reverse(array);
            return array;
        }
    }
}