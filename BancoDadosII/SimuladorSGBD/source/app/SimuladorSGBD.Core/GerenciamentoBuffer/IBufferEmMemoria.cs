using System.Collections.Generic;

namespace SimuladorSGBD.Core.GerenciamentoBuffer
{
    public interface IBufferEmMemoria
    {
        int NumeroPaginasNoBuffer { get; }
        void Armazenar(IPaginaEmMemoria pagina);
        IPaginaEmMemoria Obter(int indicePagina);
        IEnumerable<IResumoPagina> ListarPaginas();
    }
}