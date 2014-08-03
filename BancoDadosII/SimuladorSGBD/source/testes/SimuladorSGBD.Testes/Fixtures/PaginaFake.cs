using SimuladorSGBD.Core.GerenciamentoBuffer;

namespace SimuladorSGBD.Testes.Fixtures
{
    public class PaginaFake : IPaginaEmMemoria
    {
        public char[] Conteudo { get; set; }
        public bool Sujo { get; set; }
        public int PinCount { get; set; }
        public int UltimoAcesso { get; set; }
        public int IndicePaginaNoDisco { get; set; }
    }
}