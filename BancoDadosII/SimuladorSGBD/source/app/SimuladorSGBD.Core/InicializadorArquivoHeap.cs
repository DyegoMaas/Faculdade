using SimuladorSGBD.Core.IO;

namespace SimuladorSGBD.Core
{
    public class InicializadorArquivoHeap : IInicializadorArquivoMestre
    {
        private readonly IGerenciadorEspacoEmDisco gerenciadorEspacoEmDisco;

        public InicializadorArquivoHeap(IGerenciadorEspacoEmDisco gerenciadorEspacoEmDisco)
        {
            this.gerenciadorEspacoEmDisco = gerenciadorEspacoEmDisco;
        }

        public void Inicializar(int blocos, int bytes)
        {
            gerenciadorEspacoEmDisco.CriarArquivoSeNaoExiste(blocos, bytes);
        }
    }
}