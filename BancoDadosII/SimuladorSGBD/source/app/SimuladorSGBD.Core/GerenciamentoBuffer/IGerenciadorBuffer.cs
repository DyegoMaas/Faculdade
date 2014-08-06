using System.Collections.Generic;
using SimuladorSGBD.Core.GerenciamentoBuffer.Paginas;

namespace SimuladorSGBD.Core.GerenciamentoBuffer
{
    public interface IGerenciadorBuffer
    {
        IQuadro ObterPagina(int indice);
        void SalvarPagina(int indice);
        void AtualizarPagina(int indice, char[] conteudo);
        IEnumerable<IResumoPagina> ListarPaginas();
    }
}