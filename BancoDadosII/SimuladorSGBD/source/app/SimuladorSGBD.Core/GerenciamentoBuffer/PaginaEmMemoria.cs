namespace SimuladorSGBD.Core.GerenciamentoBuffer
{
    internal class PaginaEmMemoria : IPaginaEmMemoria
    {
        private readonly int indiceNoDisco;

        public PaginaEmMemoria(int indiceNoDisco)
        {
            this.indiceNoDisco = indiceNoDisco;
        }

        public char[] Conteudo { get; set; }
        public bool Sujo { get; set; }
        public int PinCount { get; set; }
        public int UltimoAcesso { get; set; }

        public int IndicePaginaNoDisco {
            get { return indiceNoDisco; }
        }
    }
}