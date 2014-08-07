namespace SimuladorSGBD.Core.GerenciamentoBuffer.Buffer.LogicaSubstituicao
{
    public interface ILogicaSubstituicaoFactory
    {
        ILogicaSubstituicao LRU();
        ILogicaSubstituicao MRU();
    }
}