using System.Collections.Generic;
using SimuladorSGBD.Core.GerenciamentoBuffer.Paginas;

namespace SimuladorSGBD.Core.GerenciamentoBuffer
{
    public interface IGerenciadorBuffer
    {
        IPaginaEmMemoria CarregarPagina(int indice);
        void SalvarPagina(int indice);
        void AtualizarPagina(int indicePagina, char[] conteudo);
        IEnumerable<IResumoPagina> ListarPaginas();
    }
}