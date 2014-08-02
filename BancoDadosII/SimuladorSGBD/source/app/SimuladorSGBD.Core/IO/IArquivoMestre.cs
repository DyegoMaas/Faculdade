using System;

namespace SimuladorSGBD.Core.IO
{
    public interface IArquivoMestre : IDisposable
    {
        void CriarArquivoSeNaoExiste(int blocos, int bytes);
        bool ArquivoExiste();
        IPagina CarregarPagina(int indicePagina);
    }
}