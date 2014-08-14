﻿using SimuladorSGBD.Core.GerenciamentoBuffer.Paginas;
using System.IO;
using System.Linq;
using System.Text;

namespace SimuladorSGBD.Core.IO
{
    public class GerenciadorEspacoEmDisco : IGerenciadorEspacoEmDisco
    {
        private const int TamanhoPaginas = 128;
        private readonly FileInfo arquivo;

        public GerenciadorEspacoEmDisco(IConfiguracaoIO configuracaoIO)
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

        public IPagina CarregarPagina(int indicePagina)
        {
            var buffer = new byte[TamanhoPaginas];
            using (var stream = arquivo.OpenRead())
            {
                var offset = indicePagina * TamanhoPaginas;
                stream.Seek(offset, SeekOrigin.Begin);
                stream.Read(buffer, 0, buffer.Length);
            }

            return new Pagina
            {
                Conteudo = Encoding.ASCII.GetChars(buffer, 0, buffer.Length)
            };
        }

        public void SalvarPagina(int indicePagina, IPagina pagina)
        {
            var buffer = Encoding.ASCII.GetBytes(pagina.Conteudo, 0, pagina.Conteudo.Length);
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