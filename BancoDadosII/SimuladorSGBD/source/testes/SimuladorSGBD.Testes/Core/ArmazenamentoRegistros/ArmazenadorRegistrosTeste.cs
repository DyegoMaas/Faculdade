using System;
using Moq;
using SimuladorSGBD.Core.GerenciamentoBuffer;
using SimuladorSGBD.Testes.Fixtures;
using Xunit;

namespace SimuladorSGBD.Testes.Core.ArmazenamentoRegistros
{
    public class ArmazenadorRegistrosTeste
    {
        [Fact]
        public void cadastrando_um_valor_vazio()
        {
            var quadroOriginal = new QuadroTesteBuilder().Construir();

            var mockGerenciadorBuffer = new Mock<IGerenciadorBuffer>();
            mockGerenciadorBuffer.Setup(gb => gb.ObterQuadro(It.IsAny<int>()))
                .Returns(quadroOriginal);

            var armazenadorRegistros = new ArmazenadorRegistros(mockGerenciadorBuffer.Object);
            armazenadorRegistros.Inserir(new Registro {Conteudo = new char[0]});

            mockGerenciadorBuffer.Verify(gb => gb.ObterQuadro(0), Times.Once());
            mockGerenciadorBuffer.Verify(gb => gb.AtualizarPagina(0, It.Is<char[]>(array => ValidarFinal(array) && ValidarInicio(array))), Times.Once());
            mockGerenciadorBuffer.Verify(gb => gb.LiberarPagina(0, true), Times.Once());
            //{
            //    var conteudo = new String(array);
            //    return conteudo.EndsWith("a");
            //}
            //cadastrar um valor vazio
            //deve alocar um caractere coringa na posicao do arquivo
            //deve alocar um ponteiro no final do arquivo indicando a posicao e tamanho 0
        }

        private bool ValidarInicio(char[] array)
        {
            return array[0] == CaractereCoringa 
                && array[1] == '0';
        }

        public const char CaractereCoringa = 'A'; //TODO escolher o wildcard

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

            var copiaConteudoQuadro = quadro.Pagina.Conteudo.Clone() as char[];
            AdicionarRegistroNoQuadro(ref copiaConteudoQuadro, registro);
            gerenciadorBuffer.AtualizarPagina(0, copiaConteudoQuadro);
            gerenciadorBuffer.LiberarPagina(0, true);
        }

        private void AdicionarRegistroNoQuadro(ref char[] conteudoQuadro, Registro registro)
        {
            //conteudo
            var indiceQuadro = 0;
            Array.Copy(registro.Conteudo, 0, conteudoQuadro, indiceQuadro, conteudoQuadro.Length);

            //ponteiro
            var criarPonteiro = CriarPonteiro(registro);
            Array.Copy(criarPonteiro.ToCharArray(), 0, conteudoQuadro, 0, criarPonteiro.Length);
        }

        private string CriarPonteiro(Registro registro)
        {
            return string.Format("{0}{1}", registro.Conteudo.Length.ToString("D4"), 0.ToString("D4"));
        }
    }
}