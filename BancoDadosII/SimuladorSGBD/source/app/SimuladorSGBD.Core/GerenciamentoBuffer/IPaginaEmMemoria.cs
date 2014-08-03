
namespace SimuladorSGBD.Core.GerenciamentoBuffer
{
    public interface IPaginaEmMemoria : IPaginaComConteudo
    {
        bool Sujo { get; set; }
        int PinCount { get; set; }
        int UltimoAcesso { get; set; }
        int IndicePaginaNoDisco { get; }
    }
}