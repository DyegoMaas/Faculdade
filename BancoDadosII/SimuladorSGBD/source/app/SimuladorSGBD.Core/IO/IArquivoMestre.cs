
namespace SimuladorSGBD.Core.IO
{
    public interface IArquivoMestre
    {
        bool ExisteNoDisco { get; }
        void CriarArquivoSeNaoExiste(int blocos, int bytes);
        IPagina CarregarPagina(int indicePagina);
        void SalvarPagina(int indicePagina, IPagina pagina);
    }
}