using SimuladorSGBD.Core.IO;

namespace SimuladorSGBD.Core
{
    public class InicializadorArquivoMaster
    {
        private readonly IManipuladorArquivoMestreFactory manipuladorArquivoMestreFactory;

        public InicializadorArquivoMaster(IManipuladorArquivoMestreFactory manipuladorArquivoMestreFactory)
        {
            this.manipuladorArquivoMestreFactory = manipuladorArquivoMestreFactory;
        }

        public void Inicializar(string caminhoArquivo)
        {
            using (var arquivo = manipuladorArquivoMestreFactory.Criar())
            {
                arquivo.CriarArquivoSeNaoExiste(blocos: 20, bytes: 128);
            }
        }
    }
}