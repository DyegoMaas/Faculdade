namespace SimuladorSGBD.Core.GerenciamentoBuffer
{
    public interface IPaginaEmMemoria : Core.IPaginaComConteudo
    {
        bool Sujo { get; }
        int PinCount { get; }
        int UltimoAcesso { get; }
        int IndicePaginaNoDisco { get; set; }
    }
}