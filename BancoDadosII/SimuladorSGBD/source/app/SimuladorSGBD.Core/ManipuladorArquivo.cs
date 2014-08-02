using System;
using System.IO;

namespace SimuladorSGBD.Core
{
    public class ManipuladorArquivo : IManipuladorArquivo
    {
        private readonly FileInfo arquivo;

        public ManipuladorArquivo(string caminhoArquivo)
        {
            arquivo = new FileInfo(caminhoArquivo);
        }

        public void CriarArquivoSeNaoExiste(int blocos, int bytes)
        {
            if (ArquivoExiste())
                return;

            using (var stream = arquivo.Create())
            {
                for (int i = 0; i < blocos; i++)
                {
                    CriarBlocoVazio(stream, bytes);
                }
            }
        }

        public bool ArquivoExiste()
        {
            return arquivo.Exists;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private void CriarBlocoVazio(Stream stream, int bytes)
        {
            var buffer = new byte[bytes];
            Array.Clear(buffer, 0, buffer.Length);
            stream.Write(buffer, 0, buffer.Length);
        }
    }
}