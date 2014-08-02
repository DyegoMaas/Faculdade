namespace SimuladorSGBD.Core
{
    public interface IPagina
    {
        char[] Dados { get; }
        bool Sujo { get; }
        int PinCount { get; }
        int UltimoAcesso { get; }
    }
}