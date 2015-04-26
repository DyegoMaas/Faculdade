using System.Collections.Generic;
using SimuladorSGBD.Core.GerenciamentoBuffer.Paginas;

namespace SimuladorSGBD.Core.GerenciamentoBuffer.Buffer
{
    public interface IPoolDeBuffers
    {
        int NumeroPaginasNoBuffer { get; }
        void Armazenar(IQuadro quadro);
        IQuadro Obter(int indicePagina);
        IEnumerable<IResumoPagina> ListarQuadros();
        void Remover(int indicePagina);
    }
}