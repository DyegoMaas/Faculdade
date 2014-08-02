using System.IO;

namespace SimuladorSGBD.Core
{
    public class ManipuladorArquivos : IManipuladorArquivos
    {
        public void CriarArquivo(string caminhoArquivo)
        {
            if (ArquivoExiste(caminhoArquivo))
                return;

            var arquivo = new FileInfo(caminhoArquivo);
            arquivo.Create();
        }

        public bool ArquivoExiste(string caminhoArquivo)
        {
            var arquivo = new FileInfo(caminhoArquivo);
            return arquivo.Exists;
        }

        public void CriarBlocoVazio(int bytes)
        {
            throw new System.NotImplementedException();
        }
    }
}