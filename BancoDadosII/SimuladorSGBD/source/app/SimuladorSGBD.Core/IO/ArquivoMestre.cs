using System;
using System.IO;

namespace SimuladorSGBD.Core.IO
{
    public class ArquivoMestre : IArquivoMestre
    {
        private readonly FileInfo arquivo;

        public ArquivoMestre(string caminhoArquivo)
        {
            arquivo = new FileInfo(caminhoArquivo);
        }
        
        public bool ExisteNoDisco
        {
            get { return arquivo.Exists; }
        }

        public void CriarArquivoSeNaoExiste(int blocos, int bytes)
        {
            if (ExisteNoDisco)
                return;

            using (var stream = arquivo.Create())
            {
                for (int i = 0; i < blocos; i++)
                {
                    CriarBlocoVazio(stream, bytes);
                }
            }
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