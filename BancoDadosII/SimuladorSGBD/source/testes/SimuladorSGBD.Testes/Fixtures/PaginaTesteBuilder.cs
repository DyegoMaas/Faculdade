using System.Linq;
using SimuladorSGBD.Core.GerenciamentoBuffer;

namespace SimuladorSGBD.Testes.Fixtures
{
    public class PaginaTesteBuilder
    {
        private char[] conteudo = Enumerable.Repeat('a', 128).ToArray();
        private int indicePaginaNoDisco = 0;
        private int pinCount = 0;
        private bool sujo;
        private int ultimoAcesso = 0;

        private PaginaTesteBuilder ComConteudo(char[] conteudo)
        {
            this.conteudo = conteudo;
            return this;
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
                Dados = conteudo,
                IndicePaginaNoDisco = indicePaginaNoDisco,
                PinCount = pinCount,
                Sujo = sujo,
                UltimoAcesso = ultimoAcesso
            };
        }
    }
}