using System;

namespace SimuladorSGBD.Core
{
    public interface IManipuladorArquivo : IDisposable
    {
        void CriarArquivoSeNaoExiste(int blocos, int bytes);
        bool ArquivoExiste();
    }
}