using SimuladorSGBD.Core.ArmazenamentoRegistros;
using SimuladorSGBD.Core.GerenciamentoBuffer;
using System;
using System.Text;
using Xunit;

namespace SimuladorSGBD.Testes.Core.ArmazenamentoRegistros
{
    public class ArmazenadorRegistrosTeste
    {
        [Fact]
        public void teste()
        {
            
        }
    }

    public class Registro
    {
        public byte[] Conteudo { get; set; }
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