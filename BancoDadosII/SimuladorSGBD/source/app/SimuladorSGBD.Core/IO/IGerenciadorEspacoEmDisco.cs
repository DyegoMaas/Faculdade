
namespace SimuladorSGBD.Core.IO
{
    public interface IGerenciadorEspacoEmDisco
    {
        bool ExisteNoDisco { get; }
        void CriarArquivoSeNaoExiste(int blocos, int bytes);
        IPagina CarregarPagina(int indicePagina);
        void SalvarPagina(int indicePagina, IPagina pagina);
    }
}