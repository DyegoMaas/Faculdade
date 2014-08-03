namespace SimuladorSGBD.Core.GerenciamentoBuffer
{
    internal class PaginaEmMemoria : IPaginaEmMemoria
    {
        public char[] Dados { get; set; }
        public bool Sujo { get; set; }
        public int PinCount { get; set; }
        public int UltimoAcesso { get; set; }
    }
}