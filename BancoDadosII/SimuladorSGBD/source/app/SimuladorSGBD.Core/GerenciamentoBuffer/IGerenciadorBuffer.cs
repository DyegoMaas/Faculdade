using System.Collections.Generic;

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