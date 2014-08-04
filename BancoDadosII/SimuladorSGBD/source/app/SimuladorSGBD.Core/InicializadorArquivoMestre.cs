using SimuladorSGBD.Core.IO;

namespace SimuladorSGBD.Core
{
    public class InicializadorArquivoMestre : IInicializadorArquivoMestre
    {
        private readonly IArquivoMestre arquivoMestre;

        public InicializadorArquivoMestre(IArquivoMestre arquivoMestre)
        {
            this.arquivoMestre = arquivoMestre;
        }

        public void Inicializar()
        {
            arquivoMestre.CriarArquivoSeNaoExiste(blocos: 20, bytes: 128);
        }
    }
}