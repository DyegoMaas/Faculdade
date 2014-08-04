using System.Collections.Generic;
using SimuladorSGBD.Core.GerenciamentoBuffer.Paginas;

namespace SimuladorSGBD.Core.GerenciamentoBuffer
{
    public interface IGerenciadorBuffer
    {
        void InicializarBuffer();
        IPaginaEmMemoria CarregarPagina(int indice);
        IPaginaEmMemoria LerPagina(int indicePagina);
        void SalvarPagina(int indice);
        void AtualizarPagina(int indicePagina, char[] conteudo);
        IEnumerable<IResumoPagina> ListarPaginas();
    }
}