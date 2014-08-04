using System.Collections.Generic;
using System.Linq;
using SimuladorSGBD.Core.GerenciamentoBuffer.Paginas;

namespace SimuladorSGBD.Core.GerenciamentoBuffer.Buffer
{
    public class BufferEmMemoria : IBufferEmMemoria
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

        public IEnumerable<IResumoPagina> ListarPaginas()
        {
            return buffer.Values.Select(b => new ResumoPagina
            {
                Conteudo = b.Conteudo,
                IndiceNoDisco = b.IndicePaginaNoDisco,
                PinCount = b.PinCount,
                Sujo = b.Sujo
            });
        }
    }
}