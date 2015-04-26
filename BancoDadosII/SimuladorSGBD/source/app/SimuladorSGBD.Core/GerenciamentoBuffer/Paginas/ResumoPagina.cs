using System.Text;

namespace SimuladorSGBD.Core.GerenciamentoBuffer.Paginas
{
    internal class ResumoPagina : IResumoPagina
    {
        public byte[] Conteudo { get; set; }
        public int IndiceNoDisco { get; set; }
        public int PinCount { get; set; }
        public bool Sujo { get; set; }

        public override string ToString()
        {
            var conteudo = Encoding.UTF8.GetChars(Conteudo);
            return string.Format("Índice: {0}; pin-count: {1}; sujo: {2}, conteúdo do bloco: {3}", IndiceNoDisco, PinCount, Sujo, conteudo);
        }
    }
}