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

        public void Inicializar()
        {
            gerenciadorEspacoEmDisco.CriarArquivoSeNaoExiste(blocos: 20, bytes: 128);
        }
    }
}