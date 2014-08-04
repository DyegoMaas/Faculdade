using System.Linq;
using SimuladorSGBD.Core.GerenciamentoBuffer.Paginas;

namespace SimuladorSGBD.Testes.Fixtures
{
    public class PaginaTesteBuilder
    {
        private char[] conteudo = Enumerable.Repeat('a', 128).ToArray();
        private int indicePaginaNoDisco = 0;
        private int pinCount = 0;
        private bool sujo;
        private int ultimoAcesso = 0;

        public PaginaTesteBuilder ComConteudo(char[] conteudo)
        {
            this.conteudo = conteudo;
            return this;
        }

        public PaginaTesteBuilder PreenchidoCom(int numeroCaracteres, char caractere)
        {
            return ComConteudo(Enumerable.Repeat(caractere, numeroCaracteres).ToArray());
        }

        public PaginaTesteBuilder NoIndice(int indice)
        {
            this.indicePaginaNoDisco = indice;
            return this;
        }

        public PaginaTesteBuilder ComPinCount(int pinCount)
        {
            this.pinCount = pinCount;
            return this;
        }

        public PaginaTesteBuilder Sujo()
        {
            this.sujo = true;
            return this;
        }

        public PaginaTesteBuilder ConUltimoAcesso(int ultimoAcesso)
        {
            this.ultimoAcesso = ultimoAcesso;
            return this;
        }

        public IPaginaEmMemoria Construir()
        {
            return new PaginaFake
            {
                Conteudo = conteudo,
                IndicePaginaNoDisco = indicePaginaNoDisco,
                PinCount = pinCount,
                Sujo = sujo,
                UltimoAcesso = ultimoAcesso
            };
        }

        private class PaginaFake : IPaginaEmMemoria
        {
            public char[] Conteudo { get; set; }
            public bool Sujo { get; set; }
            public int PinCount { get; set; }
            public int UltimoAcesso { get; set; }
            public int IndicePaginaNoDisco { get; set; }
        }
    }
}