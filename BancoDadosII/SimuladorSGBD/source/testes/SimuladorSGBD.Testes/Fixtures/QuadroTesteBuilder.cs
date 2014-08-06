using System.Linq;
using SimuladorSGBD.Core;
using SimuladorSGBD.Core.GerenciamentoBuffer.Paginas;

namespace SimuladorSGBD.Testes.Fixtures
{
    public class QuadroTesteBuilder
    {
        private char[] conteudo = Enumerable.Repeat('a', 128).ToArray();
        private int indicePaginaNoDisco = 0;
        private int pinCount = 0;
        private bool sujo;
        private int ultimoAcesso = 0;

        public QuadroTesteBuilder ComConteudo(char[] conteudo)
        {
            this.conteudo = conteudo;
            return this;
        }

        public QuadroTesteBuilder PreenchidoCom(int numeroCaracteres, char caractere)
        {
            return ComConteudo(Enumerable.Repeat(caractere, numeroCaracteres).ToArray());
        }

        public QuadroTesteBuilder NoIndice(int indice)
        {
            this.indicePaginaNoDisco = indice;
            return this;
        }

        public QuadroTesteBuilder ComPinCount(int pinCount)
        {
            this.pinCount = pinCount;
            return this;
        }

        public QuadroTesteBuilder Sujo(bool sujo = true)
        {
            this.sujo = sujo;
            return this;
        }

        public QuadroTesteBuilder ConUltimoAcesso(int ultimoAcesso)
        {
            this.ultimoAcesso = ultimoAcesso;
            return this;
        }

        public IQuadro Construir()
        {
            return new QuadroFake
            {
                Pagina = new PaginaFake {Conteudo = conteudo},
                IndicePaginaNoDisco = indicePaginaNoDisco,
                PinCount = pinCount,
                Sujo = sujo,
                UltimoAcesso = ultimoAcesso
            };
        }

        private class QuadroFake : IQuadro
        {
            public IPagina Pagina { get; set; }
            public bool Sujo { get; set; }
            public int PinCount { get; set; }
            public int UltimoAcesso { get; set; }
            public int IndicePaginaNoDisco { get; set; }
        }

        private class PaginaFake : IPagina
        {
            public char[] Conteudo { get; set; }
        }
    }
}