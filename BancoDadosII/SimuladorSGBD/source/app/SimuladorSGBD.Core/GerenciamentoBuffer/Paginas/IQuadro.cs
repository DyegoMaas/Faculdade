
namespace SimuladorSGBD.Core.GerenciamentoBuffer.Paginas
{
    public interface IQuadro
    {
        IPagina Pagina { get; set; }
        bool Sujo { get; set; }
        int PinCount { get; set; }
        int UltimoAcesso { get; set; }
        int IndicePaginaNoDisco { get; }
    }
}