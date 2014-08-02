using System;

namespace SimuladorSGBD.Core.IO
{
    public interface IManipuladorArquivo : IDisposable
    {
        void CriarArquivoSeNaoExiste(int blocos, int bytes);
        bool ArquivoExiste();
    }
}