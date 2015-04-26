using SimuladorSGBD.Core.GerenciamentoBuffer.Buffer.LogicaSubstituicao.PinCount;

namespace SimuladorSGBD.Core.GerenciamentoBuffer
{
    public interface ILogicaSubstituicao : IPinCountChangeObserver
    {
        int Selecionar();
    }
}