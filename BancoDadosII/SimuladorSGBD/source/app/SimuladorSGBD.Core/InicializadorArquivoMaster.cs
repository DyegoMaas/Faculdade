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
            if (manipuladorArquivos.ArquivoExiste(caminhoArquivo))
                return;

            manipuladorArquivos.CriarArquivo(caminhoArquivo);
            CriarBlocos(blocos:20, bytes:128);
        }

        private void CriarBlocos(int blocos, int bytes)
        {
            for (int i = 0; i < blocos; i++)
            {
                manipuladorArquivos.CriarBlocoVazio(bytes);
            }
        }
    }
}