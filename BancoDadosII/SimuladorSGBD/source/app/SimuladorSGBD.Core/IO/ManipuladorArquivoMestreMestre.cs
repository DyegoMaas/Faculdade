using System;
using System.IO;

namespace SimuladorSGBD.Core.IO
{
    public class ManipuladorArquivoMestreMestre : IManipuladorArquivoMestre
    {
        private readonly FileInfo arquivo;

        public ManipuladorArquivoMestreMestre(string caminhoArquivo)
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

        public IPagina CarregarPagina(int indicePagina)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }

        private void CriarBlocoVazio(Stream stream, int bytes)
        {
            var buffer = new byte[bytes];
            Array.Clear(buffer, 0, buffer.Length);
            stream.Write(buffer, 0, buffer.Length);
        }
    }
}