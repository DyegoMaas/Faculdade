namespace SimuladorSGBD.Core
{
    public interface IPaginaEmMemoria : IPaginaComDados
    {
        bool Sujo { get; }
        int PinCount { get; }
        int UltimoAcesso { get; }
    }
}