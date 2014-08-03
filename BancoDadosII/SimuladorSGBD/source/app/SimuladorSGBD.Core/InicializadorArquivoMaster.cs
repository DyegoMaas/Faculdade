using SimuladorSGBD.Core.IO;

namespace SimuladorSGBD.Core
{
    public class InicializadorArquivoMaster
    {
        private readonly IArquivoMestre arquivoMestre;

        public InicializadorArquivoMaster(IArquivoMestre arquivoMestre)
        {
            this.arquivoMestre = arquivoMestre;
        }

        public void Inicializar(string caminhoArquivo)
        {
            arquivoMestre.CriarArquivoSeNaoExiste(blocos: 20, bytes: 128);
        }
    }
}