namespace SimuladorSGBD.Core.GerenciamentoBuffer.Buffer.LogicaSubstituicao.PinCount
{
    public interface IPinCountSubject
    {
        void Registrar(IPinCountChangeListener pinCountChangeListener);
    }
}