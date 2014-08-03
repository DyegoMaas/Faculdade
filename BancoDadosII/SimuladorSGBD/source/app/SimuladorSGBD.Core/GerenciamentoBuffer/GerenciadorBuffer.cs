using System;
using SimuladorSGBD.Core.IO;

namespace SimuladorSGBD.Core.GerenciamentoBuffer
{
    public class GerenciadorBuffer
    {
        private readonly IArquivoMestre arquivoMestre;
        private readonly IBufferEmMemoria buffer;
        private readonly IConfiguaracaoBuffer configuaracaoBuffer;

        public GerenciadorBuffer(IArquivoMestre arquivoMestre, IBufferEmMemoria buffer, IConfiguaracaoBuffer configuaracaoBuffer)
        {
            this.arquivoMestre = arquivoMestre;
            this.buffer = buffer;
            this.configuaracaoBuffer = configuaracaoBuffer;
        }

        public IPaginaEmMemoria CarregarPagina(int indice)
        {
            if(BufferEstaCheio())
                throw new InvalidOperationException("Não é possível carregar novas páginas ao buffer. O buffer está cheio.");

            var paginaBuffer = buffer.Obter(indice);
            if (paginaBuffer != null)
                return paginaBuffer;

            var pagina = CarregarPaginaDoDisco(indice);
            ArmazenarNoBuffer(pagina);

            return pagina;
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

        private bool BufferEstaCheio()
        {
            return buffer.NumeroPaginasNoBuffer == configuaracaoBuffer.LimiteDePaginasEmMemoria;
        }

        private void ArmazenarNoBuffer(PaginaEmMemoria paginaEmMemoria)
        {
            buffer.Armazenar(paginaEmMemoria);
        }
    }
}