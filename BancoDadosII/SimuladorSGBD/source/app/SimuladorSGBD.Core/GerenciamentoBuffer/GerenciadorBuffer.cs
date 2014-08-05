using System;
using System.Collections.Generic;
using SimuladorSGBD.Core.GerenciamentoBuffer.Buffer;
using SimuladorSGBD.Core.GerenciamentoBuffer.Paginas;
using SimuladorSGBD.Core.IO;

namespace SimuladorSGBD.Core.GerenciamentoBuffer
{
    public class GerenciadorBuffer : IGerenciadorBuffer
    {
        private readonly IGerenciadorEspacoEmDisco gerenciadorEspacoEmDisco;
        private readonly IPoolDeBuffers buffer;
        private readonly IConfiguracaoBuffer configuracaoBuffer;

        public GerenciadorBuffer(IGerenciadorEspacoEmDisco gerenciadorEspacoEmDisco, IPoolDeBuffers buffer, IConfiguracaoBuffer configuracaoBuffer)
        {
            this.gerenciadorEspacoEmDisco = gerenciadorEspacoEmDisco;
            this.buffer = buffer;
            this.configuracaoBuffer = configuracaoBuffer;
        }

        public void InicializarBuffer()
        {
            for (int indicePagina = 0; indicePagina < configuracaoBuffer.LimiteDePaginasEmMemoria; indicePagina++)
            {
                var pagina = CarregarPaginaDoDisco(indicePagina);
                var quadro = MontarNovoQuadro(pagina, indicePagina);
                buffer.Armazenar(quadro);
            }
        }

        public IQuadro LerPagina(int indice)
        {
            var quadroBuffer = buffer.Obter(indice);
            if (quadroBuffer != null)
            {
                quadroBuffer.PinCount++;
                return quadroBuffer;
            }

            return CarregarPagina(indice);
        }

        private IQuadro CarregarPagina(int indice)
        {
            var quadroBuffer = buffer.Obter(indice);
            if (quadroBuffer == null && BufferEstaCheio())
                throw new InvalidOperationException("Não é possível carregar novas páginas ao buffer. O buffer está cheio.");

            var pagina = CarregarPaginaDoDisco(indice);
            var quadro = MontarNovoQuadro(pagina, indice);
            ArmazenarNoBuffer(quadro);

            return quadro;
        }

        public void SalvarPagina(int indice)
        {
            var quadro = buffer.Obter(indice);
            gerenciadorEspacoEmDisco.SalvarPagina(indice, quadro.Pagina);
        }

        public void AtualizarPagina(int indice, char[] conteudo)
        {
            var quadro = buffer.Obter(indice);
            quadro.Sujo = true;
            quadro.Pagina.Conteudo = conteudo;
        }
        
        public IEnumerable<IResumoPagina> ListarPaginas()
        {
            return buffer.ListarPaginas();
        }

        private IPagina CarregarPaginaDoDisco(int indice)
        {
            return gerenciadorEspacoEmDisco.CarregarPagina(indice);
        }

        private Quadro MontarNovoQuadro(IPagina pagina, int indicePagina)
        {
            var quadro = new Quadro(indicePagina)
            {
                Pagina = pagina,
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