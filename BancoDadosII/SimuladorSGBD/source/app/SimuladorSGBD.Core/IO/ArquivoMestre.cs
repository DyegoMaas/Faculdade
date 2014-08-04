using System;
using System.IO;
using System.Linq;
using System.Text;
using SimuladorSGBD.Core.GerenciamentoBuffer;

namespace SimuladorSGBD.Core.IO
{
    public class ArquivoMestre : IArquivoMestre
    {
        private const int TamanhoPaginas = 128;
        private readonly FileInfo arquivo;

        public ArquivoMestre(IConfiguracaoIO configuracaoIO)
        {
            arquivo = new FileInfo(configuracaoIO.CaminhoArquivoMestre);
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

        public IPaginaComConteudo CarregarPagina(int indicePagina)
        {
            var buffer = new byte[TamanhoPaginas];
            using (var stream = arquivo.OpenRead())
            {
                var offset = indicePagina * TamanhoPaginas;
                stream.Seek(offset, SeekOrigin.Begin);
                stream.Read(buffer, 0, buffer.Length);
            }

            return new PaginaEmMemoria(indicePagina)
            {
                Conteudo = Encoding.ASCII.GetChars(buffer, 0, buffer.Length)
            };
        }

        public void SalvarPagina(int indicePagina, IPaginaComConteudo paginaComConteudo)
        {
            var buffer = Encoding.ASCII.GetBytes(paginaComConteudo.Conteudo, 0, paginaComConteudo.Conteudo.Length);
            using (var stream = arquivo.OpenWrite())
            {
                var offset = indicePagina * TamanhoPaginas;
                stream.Seek(offset, SeekOrigin.Begin);
                stream.Write(buffer, 0, buffer.Length);
            }
        }

        private void CriarBlocoVazio(Stream stream, int bytes)
        {
            var buffer = Encoding.ASCII.GetBytes(Enumerable.Repeat('0', bytes).ToArray());
            stream.Write(buffer, 0, buffer.Length);
        }
    }
}