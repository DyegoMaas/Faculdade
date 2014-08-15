using System.Collections.Generic;
using System.Linq;
using SimuladorSGBD.Core.GerenciamentoBuffer.Buffer.LogicaSubstituicao.PinCount;

namespace SimuladorSGBD.Core.GerenciamentoBuffer.Buffer.LogicaSubstituicao
{
    public class MostRecentlyUsed : ILogicaSubstituicao, IPinCountChangeObserver
    {
        private readonly Stack<int> quadrosDisponiveisSubstituicao = new Stack<int>();

        public int Selecionar()
        {
            if (quadrosDisponiveisSubstituicao.Any())
                return quadrosDisponiveisSubstituicao.Pop();
            return -1;
        }

        public void NotificarIncrementoPinCount(int indice, int novoPinCount)
        {
        }

        public void NotificarDecrementoPinCount(int indice, int novoPinCount)
        {
            if (novoPinCount == 0)
                quadrosDisponiveisSubstituicao.Push(indice);
        }

        public void NotificarNovoQuadroComPinCountZero(int indice)
        {
        }
    }
}