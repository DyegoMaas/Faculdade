using System;

namespace SimuladorSGBD.Core.GerenciamentoBuffer.Buffer.LogicaSubstituicao
{
    public class LogicaSubstituicaoFactory : ILogicaSubstituicaoFactory
    {
        private readonly Lazy<ILogicaSubstituicao> lru = new Lazy<ILogicaSubstituicao>(() => new LeastRecentlyUsed());
        private readonly Lazy<ILogicaSubstituicao> mru = new Lazy<ILogicaSubstituicao>(() => new MostRecentlyUsed());

        public ILogicaSubstituicao LRU()
        {
            return lru.Value;
        }

        public ILogicaSubstituicao MRU()
        {
            return mru.Value;
        }
    }
}