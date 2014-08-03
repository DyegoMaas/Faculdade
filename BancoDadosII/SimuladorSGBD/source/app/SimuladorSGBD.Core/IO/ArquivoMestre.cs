using System;
using System.IO;
using System.Text;

namespace SimuladorSGBD.Core.IO
{
    public class ArquivoMestre : IArquivoMestre
    {
        private const int TamanhoPaginas = 128;
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
            var buffer = new byte[TamanhoPaginas];
            using (var stream = arquivo.OpenRead())
            {
                var offset = indicePagina * buffer.Length;
                stream.Seek(offset, SeekOrigin.Begin);
                stream.Read(buffer, 0, buffer.Length);
            }

            return new Pagina
            {
                Dados = Encoding.ASCII.GetChars(buffer, 0, buffer.Length),
                PinCount = 0,
                Sujo = false,
                UltimoAcesso = 0
            };
        }

        private void CriarBlocoVazio(Stream stream, int bytes)
        {
            var buffer = new byte[bytes];
            Array.Clear(buffer, 0, buffer.Length);
            stream.Write(buffer, 0, buffer.Length);
        }
    }
}