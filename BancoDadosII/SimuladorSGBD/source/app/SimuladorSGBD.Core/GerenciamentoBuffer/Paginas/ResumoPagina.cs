using System;

namespace SimuladorSGBD.Core.GerenciamentoBuffer.Paginas
{
    internal class ResumoPagina : IResumoPagina
    {
        public char[] Conteudo { get; set; }
        public int IndiceNoDisco { get; set; }
        public int PinCount { get; set; }
        public bool Sujo { get; set; }

        public override string ToString()
        {
            return string.Format("Índice: {0}; pin-count: {1}; sujo: {2}, conteúdo do bloco: {3}", IndiceNoDisco, PinCount, Sujo, new String(Conteudo));
        }
    }
}