using System;

namespace SimuladorSGBD.Core.IO
{
    public interface IManipuladorArquivoMestre : IDisposable
    {
        void CriarArquivoSeNaoExiste(int blocos, int bytes);
        bool ArquivoExiste();
        IPagina CarregarPagina(int indicePagina);
    }
}