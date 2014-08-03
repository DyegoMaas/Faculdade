namespace SimuladorSGBD.Core.GerenciamentoBuffer
{
    public interface IPaginaEmMemoria : Core.IPaginaComDados
    {
        bool Sujo { get; }
        int PinCount { get; }
        int UltimoAcesso { get; }
        int IndicePaginaNoDisco { get; set; }
    }
}