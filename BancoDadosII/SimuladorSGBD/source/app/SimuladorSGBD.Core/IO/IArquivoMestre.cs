
namespace SimuladorSGBD.Core.IO
{
    public interface IArquivoMestre
    {
        bool ExisteNoDisco { get; }
        void CriarArquivoSeNaoExiste(int blocos, int bytes);
        IPaginaComDados CarregarPagina(int indicePagina);
    }
}