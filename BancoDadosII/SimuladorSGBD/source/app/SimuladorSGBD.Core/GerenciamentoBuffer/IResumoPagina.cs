namespace SimuladorSGBD.Core.GerenciamentoBuffer
{
    public interface IResumoPagina
    {
        int IndiceNoDisco { get; }
        int PinCount { get; }
        bool Sujo { get; }
    }
}