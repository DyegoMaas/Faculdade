using SimuladorSGBD.Core.IO;

namespace SimuladorSGBD.Core
{
    public class InicializadorArquivoMestre : IInicializadorArquivoMestre
    {
        private readonly IGerenciadorEspacoEmDisco gerenciadorEspacoEmDisco;

        public InicializadorArquivoMestre(IGerenciadorEspacoEmDisco gerenciadorEspacoEmDisco)
        {
            this.gerenciadorEspacoEmDisco = gerenciadorEspacoEmDisco;
        }

        public void Inicializar(int blocos, int bytes)
        {
            gerenciadorEspacoEmDisco.CriarArquivoSeNaoExiste(blocos, bytes);
        }
    }
}