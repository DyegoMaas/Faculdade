using System.Collections.Generic;
using SimuladorSGBD.Core.GerenciamentoBuffer.Paginas;

namespace SimuladorSGBD.Core.GerenciamentoBuffer.Buffer
{
    public interface IBufferEmMemoria
    {
        int NumeroPaginasNoBuffer { get; }
        void Armazenar(IPaginaEmMemoria pagina);
        IPaginaEmMemoria Obter(int indicePagina);
        IEnumerable<IResumoPagina> ListarPaginas();
    }
}