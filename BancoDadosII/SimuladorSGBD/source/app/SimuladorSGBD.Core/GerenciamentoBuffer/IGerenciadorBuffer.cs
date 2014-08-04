using System.Collections.Generic;
using SimuladorSGBD.Core.GerenciamentoBuffer.Paginas;

namespace SimuladorSGBD.Core.GerenciamentoBuffer
{
    public interface IGerenciadorBuffer
    {
        void InicializarBuffer();
        IQuadro CarregarPagina(int indice);
        IQuadro LerPagina(int indicePagina);
        void SalvarPagina(int indice);
        void AtualizarPagina(int indicePagina, char[] conteudo);
        IEnumerable<IResumoPagina> ListarPaginas();
    }
}