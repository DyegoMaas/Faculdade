using SimuladorSGBD.Core;
using SimuladorSGBD.Core.GerenciamentoBuffer.Paginas;
using SimuladorSGBD.Testes.Core.ArmazenamentoRegistros;

namespace SimuladorSGBD.Testes.Fixtures
{
    public class QuadroTesteBuilder
    {
        private readonly ConteudoPaginaTesteHelper conteudoPaginaTesteHelper;
        private byte[] conteudo;
        private int indicePaginaNoDisco = 0;
        private int pinCount = 0;
        private bool sujo;
        private int ultimoAcesso = 0;

        public QuadroTesteBuilder()
        {
            conteudoPaginaTesteHelper = new ConteudoPaginaTesteHelper();
            conteudo = conteudoPaginaTesteHelper.NovoConteudo(128, 'a');
        }

        public QuadroTesteBuilder ComConteudo(byte[] conteudo)
        {
            this.conteudo = conteudo;
            return this;
        }
        
        public QuadroTesteBuilder PreenchidoCom(int numeroCaracteres, char caractere)
        {
            var conteudoGerado = conteudoPaginaTesteHelper.NovoConteudo(numeroCaracteres, caractere);
            return ComConteudo(conteudoGerado);
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

        public QuadroTesteBuilder ComUltimoAcesso(int ultimoAcesso)
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
            public byte[] Conteudo { get; set; }
        }
    }
}