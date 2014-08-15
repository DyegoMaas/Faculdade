
namespace SimuladorSGBD.Core.GerenciamentoBuffer.Buffer.LogicaSubstituicao.PinCount
{
    public interface IPinCountChangeObserver
    {
        void NotificarIncrementoPinCount(int indice, int novoPinCount);
        void NotificarDecrementoPinCount(int indice, int novoPinCount);
        void NotificarNovoQuadroComPinCountZero(int indice);
    }
}