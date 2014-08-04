using System;
using System.Collections.Generic;
using SimuladorSGBD.Core.GerenciamentoBuffer.Buffer;
using SimuladorSGBD.Core.GerenciamentoBuffer.Paginas;
using SimuladorSGBD.Core.IO;

namespace SimuladorSGBD.Core.GerenciamentoBuffer
{
    public class GerenciadorBuffer : IGerenciadorBuffer
    {
        private readonly IArquivoMestre arquivoMestre;
        private readonly IBufferEmMemoria buffer;
        private readonly IConfiguracaoBuffer configuracaoBuffer;

        public GerenciadorBuffer(IArquivoMestre arquivoMestre, IBufferEmMemoria buffer, IConfiguracaoBuffer configuracaoBuffer)
        {
            this.arquivoMestre = arquivoMestre;
            this.buffer = buffer;
            this.configuracaoBuffer = configuracaoBuffer;
        }

        public void InicializarBuffer()
        {
            for (int indicePagina = 0; indicePagina < configuracaoBuffer.LimiteDePaginasEmMemoria; indicePagina++)
            {
                var paginaCarregadaDoDisco = CarregarPaginaDoDisco(indicePagina);
                buffer.Armazenar(paginaCarregadaDoDisco);
            }
        }
        
        public IPaginaEmMemoria CarregarPagina(int indice)
        {
            var paginaBuffer = buffer.Obter(indice);
            if (paginaBuffer == null && BufferEstaCheio())
                throw new InvalidOperationException("Não é possível carregar novas páginas ao buffer. O buffer está cheio.");

            var pagina = CarregarPaginaDoDisco(indice);
            ArmazenarNoBuffer(pagina);

            return pagina;
        }

        public IPaginaEmMemoria LerPagina(int indice)
        {
            var paginaBuffer = buffer.Obter(indice);
            if (paginaBuffer != null)
                return paginaBuffer;

            return CarregarPagina(indice);
        }

        public void SalvarPagina(int indice)
        {
            var pagina = buffer.Obter(indice);
            arquivoMestre.SalvarPagina(indice, pagina);
        }

        public void AtualizarPagina(int indicePagina, char[] conteudo)
        {
            var pagina = buffer.Obter(indicePagina);
            pagina.Sujo = true;
            pagina.Conteudo = conteudo;
        }
        
        public IEnumerable<IResumoPagina> ListarPaginas()
        {
            return buffer.ListarPaginas();
        }

        private PaginaEmMemoria CarregarPaginaDoDisco(int indice)
        {
            var paginaEmMemoria = new PaginaEmMemoria(indice)
            {
                Conteudo = arquivoMestre.CarregarPagina(indice).Conteudo,
                PinCount = 0,
                UltimoAcesso = 0
            };
            return paginaEmMemoria;
        }

        private bool BufferEstaCheio()
        {
            return buffer.NumeroPaginasNoBuffer == configuracaoBuffer.LimiteDePaginasEmMemoria;
        }

        private void ArmazenarNoBuffer(PaginaEmMemoria paginaEmMemoria)
        {
            buffer.Armazenar(paginaEmMemoria);
        }
    }
}