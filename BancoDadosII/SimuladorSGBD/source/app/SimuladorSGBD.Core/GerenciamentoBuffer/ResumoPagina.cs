namespace SimuladorSGBD.Core.GerenciamentoBuffer
{
    internal class ResumoPagina : IResumoPagina
    {
        public int IndiceNoDisco { get; set; }
        public int PinCount { get; set; }
        public bool Sujo { get; set; }

        public override string ToString()
        {
            return string.Format("Índice: {0}; pin-count: {1}; sujo: {2}", IndiceNoDisco, PinCount, Sujo);
        }
    }
}