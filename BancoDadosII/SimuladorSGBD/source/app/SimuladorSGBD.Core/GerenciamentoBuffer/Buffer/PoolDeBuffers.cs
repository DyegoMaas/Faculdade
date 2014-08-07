using System.Collections.Generic;
using System.Linq;
using SimuladorSGBD.Core.GerenciamentoBuffer.Paginas;

namespace SimuladorSGBD.Core.GerenciamentoBuffer.Buffer
{
    internal class PoolDeBuffers : IPoolDeBuffers, IBufferContainer
    {
        public IDictionary<int, IQuadro> Buffer { get; private set; }

        public PoolDeBuffers()
        {
            Buffer = new Dictionary<int, IQuadro>();
        }

        public int NumeroPaginasNoBuffer
        {
            get { return Buffer.Count; }
        }

        public void Armazenar(IQuadro quadro)
        {
            Buffer[quadro.IndicePaginaNoDisco] = quadro;
        }

        public IQuadro Obter(int indicePagina)
        {
            if(Buffer.ContainsKey(indicePagina))
                return Buffer[indicePagina];
            return null;
        }

        public void Remover(int indicePagina)
        {
            if (Buffer.ContainsKey(indicePagina))
                Buffer.Remove(indicePagina);
        }

        public IEnumerable<IResumoPagina> ListarQuadros()
        {
            return Buffer.Values.Select(b => new ResumoPagina
            {
                Conteudo = b.Pagina.Conteudo,
                IndiceNoDisco = b.IndicePaginaNoDisco,
                PinCount = b.PinCount,
                Sujo = b.Sujo
            });
        }
    }

    internal class Buff
    {
        public IQuadro[] Buffer { get; private set; }

        public Buff(IConfiguracaoBuffer configuracaoBuffer)
        {
            this.Buffer = new IQuadro[configuracaoBuffer.LimiteDePaginasEmMemoria];
        }
    }

    internal  class LogicaSubstituicaoLRU : ILogicaSubstituicao
    {
        private readonly Buff buff;

        public LogicaSubstituicaoLRU(Buff buff)
        {
            this.buff = buff;
        }

        public int Selecionar()
        {
            return 0;
        }
    }
}