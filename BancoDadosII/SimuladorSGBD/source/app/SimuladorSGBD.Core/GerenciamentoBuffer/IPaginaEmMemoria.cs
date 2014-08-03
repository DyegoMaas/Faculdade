namespace SimuladorSGBD.Core.GerenciamentoBuffer
{
    public interface IPaginaEmMemoria : IPaginaComDados
    {
        bool Sujo { get; }
        int PinCount { get; }
        int UltimoAcesso { get; }
        int IndicePagina { get; set; }
    }
}