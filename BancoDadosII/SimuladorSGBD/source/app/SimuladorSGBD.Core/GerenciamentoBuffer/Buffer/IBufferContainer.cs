using System.Collections.Generic;
using SimuladorSGBD.Core.GerenciamentoBuffer.Paginas;

namespace SimuladorSGBD.Core.GerenciamentoBuffer.Buffer
{
    public interface IBufferContainer
    {
        IDictionary<int, IQuadro> Buffer { get; }
    }
}