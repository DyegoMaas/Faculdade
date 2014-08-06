using System.Collections.Generic;
using System.Linq;
using SimuladorSGBD.Core.GerenciamentoBuffer.Paginas;

namespace SimuladorSGBD.Core.GerenciamentoBuffer.Buffer
{
    //TODO utilizar estrategia mais inteligente para o buffer
    public class PoolDeBuffers : IPoolDeBuffers
    {
        private readonly IDictionary<int, IQuadro> buffer = new Dictionary<int, IQuadro>();

        public int NumeroPaginasNoBuffer
        {
            get { return buffer.Count; }
        }

        public void Armazenar(IQuadro quadro)
        {
            buffer[quadro.IndicePaginaNoDisco] = quadro;
        }

        public IQuadro Obter(int indicePagina)
        {
            if(buffer.ContainsKey(indicePagina))
                return buffer[indicePagina];
            return null;
        }

        public void Remover(int indicePagina)
        {
            if (buffer.ContainsKey(indicePagina))
                buffer.Remove(indicePagina);
        }

        public IEnumerable<IResumoPagina> ListarQuadros()
        {
            return buffer.Values.Select(b => new ResumoPagina
            {
                Conteudo = b.Pagina.Conteudo,
                IndiceNoDisco = b.IndicePaginaNoDisco,
                PinCount = b.PinCount,
                Sujo = b.Sujo
            });
        }
    }
}