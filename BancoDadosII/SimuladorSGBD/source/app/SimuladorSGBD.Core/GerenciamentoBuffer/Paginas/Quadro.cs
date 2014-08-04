namespace SimuladorSGBD.Core.GerenciamentoBuffer.Paginas
{
    internal class Quadro : IQuadro
    {
        private readonly int indiceNoDisco;

        public Quadro(int indiceNoDisco)
        {
            this.indiceNoDisco = indiceNoDisco;
        }

        public IPagina Pagina { get; set; }
        public bool Sujo { get; set; }
        public int PinCount { get; set; }
        public int UltimoAcesso { get; set; }

        public int IndicePaginaNoDisco {
            get { return indiceNoDisco; }
        }
    }
}