namespace SimuladorSGBD.Core.GerenciamentoBuffer
{
    public interface IBuffer
    {
        int NumeroPaginasNoBuffer { get; }
        void Armazenar(IPaginaEmMemoria pagina);
        IPaginaEmMemoria Obter(int indicePagina);
    }
}