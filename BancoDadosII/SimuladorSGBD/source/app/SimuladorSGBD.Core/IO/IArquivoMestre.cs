
namespace SimuladorSGBD.Core.IO
{
    public interface IArquivoMestre
    {
        bool ExisteNoDisco { get; }
        void CriarArquivoSeNaoExiste(int blocos, int bytes);
        IPaginaComConteudo CarregarPagina(int indicePagina);
        void SalvarPagina(int indicePagina, IPaginaComConteudo paginaComConteudo);
    }
}