namespace SimuladorSGBD.Core.GerenciamentoBuffer.Paginas
{
    public interface IResumoPagina
    {
        int IndiceNoDisco { get; }
        int PinCount { get; }
        bool Sujo { get; }
    }
}