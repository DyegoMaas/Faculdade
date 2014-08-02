using SimuladorSGBD.Core.IO;

namespace SimuladorSGBD.Core
{
    public class InicializadorArquivoMaster
    {
        private readonly IManipuladorArquivos manipuladorArquivos;

        public InicializadorArquivoMaster(IManipuladorArquivos manipuladorArquivos)
        {
            this.manipuladorArquivos = manipuladorArquivos;
        }

        public void Inicializar(string caminhoArquivo)
        {
            using (var arquivo = manipuladorArquivos.Manipular(caminhoArquivo))
            {
                arquivo.CriarArquivoSeNaoExiste(blocos: 20, bytes: 128);
            }
        }
    }
}