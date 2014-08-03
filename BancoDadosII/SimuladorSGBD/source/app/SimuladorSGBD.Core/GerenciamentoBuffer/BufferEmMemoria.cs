using System.Collections.Generic;

namespace SimuladorSGBD.Core.GerenciamentoBuffer
{
    public class BufferEmMemoria : IBuffer
    {
        private readonly IDictionary<int, IPaginaEmMemoria> buffer = new Dictionary<int, IPaginaEmMemoria>();

        public int NumeroPaginasNoBuffer
        {
            get { return buffer.Count; }
        }

        public void Armazenar(IPaginaEmMemoria pagina)
        {
            buffer[pagina.IndicePaginaNoDisco] = pagina;
        }

        public IPaginaEmMemoria Obter(int indicePagina)
        {
            if(buffer.ContainsKey(indicePagina))
                return buffer[indicePagina];
            return null;
        }
    }
}