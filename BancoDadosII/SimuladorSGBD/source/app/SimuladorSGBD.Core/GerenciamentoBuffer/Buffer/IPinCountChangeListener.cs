
namespace SimuladorSGBD.Core.GerenciamentoBuffer.Buffer
{
    public interface IPinCountChangeListener
    {
        void NotificarIncrementoPinCount(int indice, int novoPinCount);
        void NotificarDecrementoPinCount(int indice, int novoPinCount);
    }
}