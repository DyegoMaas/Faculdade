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
        private readonly IPoolDeBuffers buffer;
        private readonly IConfiguracaoBuffer configuracaoBuffer;

        public GerenciadorBuffer(IArquivoMestre arquivoMestre, IPoolDeBuffers buffer, IConfiguracaoBuffer configuracaoBuffer)
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
        
        public IQuadro CarregarPagina(int indice)
        {
            var paginaBuffer = buffer.Obter(indice);
            if (paginaBuffer == null && BufferEstaCheio())
                throw new InvalidOperationException("Não é possível carregar novas páginas ao buffer. O buffer está cheio.");

            var pagina = CarregarPaginaDoDisco(indice);
            ArmazenarNoBuffer(pagina);

            return pagina;
        }

        public IQuadro LerPagina(int indice)
        {
            var paginaBuffer = buffer.Obter(indice);
            if (paginaBuffer != null)
                return paginaBuffer;

            return CarregarPagina(indice);
        }

        public void SalvarPagina(int indice)
        {
            var quadro = buffer.Obter(indice);
            arquivoMestre.SalvarPagina(indice, quadro.Pagina);
        }

        public void AtualizarPagina(int indicePagina, char[] conteudo)
        {
            var quadro = buffer.Obter(indicePagina);
            quadro.Sujo = true;
            quadro.Pagina.Conteudo = conteudo;
        }
        
        public IEnumerable<IResumoPagina> ListarPaginas()
        {
            return buffer.ListarPaginas();
        }

        private Quadro CarregarPaginaDoDisco(int indice)
        {
            var quadro = new Quadro(indice)
            {
                Pagina = arquivoMestre.CarregarPagina(indice),
                PinCount = 0,
                UltimoAcesso = 0
            };
            return quadro;
        }

        private bool BufferEstaCheio()
        {
            return buffer.NumeroPaginasNoBuffer == configuracaoBuffer.LimiteDePaginasEmMemoria;
        }

        private void ArmazenarNoBuffer(Quadro quadro)
        {
            buffer.Armazenar(quadro);
        }
    }
}