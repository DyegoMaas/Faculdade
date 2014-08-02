using System;

namespace SimuladorSGBD.Core.IO
{
    public interface IArquivoMestre : IDisposable
    {
        bool ExisteNoDisco { get; }
        void CriarArquivoSeNaoExiste(int blocos, int bytes);
        IPagina CarregarPagina(int indicePagina);
    }
}