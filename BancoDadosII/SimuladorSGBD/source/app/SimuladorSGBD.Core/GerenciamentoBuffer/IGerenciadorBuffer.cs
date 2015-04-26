using SimuladorSGBD.Core.GerenciamentoBuffer.Buffer;
using SimuladorSGBD.Core.GerenciamentoBuffer.Paginas;
using System.Collections.Generic;

namespace SimuladorSGBD.Core.GerenciamentoBuffer
{
    public interface IGerenciadorBuffer
    {
        IQuadro ObterQuadro(int indice);
        void SalvarPagina(int indice);
        void AtualizarPagina(int indice, byte[] conteudo);
        IEnumerable<IResumoPagina> ListarPaginas();
        void Registrar(IBufferChangeObserver observer);
        void LiberarPagina(int indice, bool paginaFoiAlterada);
    }
}