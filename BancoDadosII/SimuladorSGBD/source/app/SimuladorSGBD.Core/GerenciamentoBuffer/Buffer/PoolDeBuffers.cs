using System;
using SimuladorSGBD.Core.GerenciamentoBuffer.Paginas;
using System.Collections.Generic;
using System.Linq;

namespace SimuladorSGBD.Core.GerenciamentoBuffer.Buffer
{
    internal class PoolDeBuffers : IPoolDeBuffers
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
            return buffer.Values.Select(b =>
            {
                var copiaConteudoQuadro = new byte[b.Pagina.Conteudo.Length];
                Array.Copy(b.Pagina.Conteudo, copiaConteudoQuadro, copiaConteudoQuadro.Length);

                return new ResumoPagina
                {
                    Conteudo = copiaConteudoQuadro,
                    IndiceNoDisco = b.IndicePaginaNoDisco,
                    PinCount = b.PinCount,
                    Sujo = b.Sujo
                };
            });
        }
    }
}