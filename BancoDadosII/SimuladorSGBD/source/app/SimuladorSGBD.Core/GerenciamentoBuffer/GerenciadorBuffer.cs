using System;
using SimuladorSGBD.Core.IO;

namespace SimuladorSGBD.Core.GerenciamentoBuffer
{
    public class GerenciadorBuffer
    {
        private readonly IArquivoMestre arquivoMestre;
        private readonly IBuffer buffer;
        private readonly IConfiguaracaoBuffer configuaracaoBuffer;

        public GerenciadorBuffer(IArquivoMestre arquivoMestre, IBuffer buffer, IConfiguaracaoBuffer configuaracaoBuffer)
        {
            this.arquivoMestre = arquivoMestre;
            this.buffer = buffer;
            this.configuaracaoBuffer = configuaracaoBuffer;
        }

        public IPaginaEmMemoria CarregarPagina(int indice)
        {
            if(BufferEstaCheio())
                throw new InvalidOperationException("Não é possível carregar novas páginas ao buffer. O buffer está cheio.");

            var paginaEmMemoria = new PaginaEmMemoria
            {
                Dados = arquivoMestre.CarregarPagina(indice).Dados,
                PinCount = 0,
                Sujo = false,
                UltimoAcesso = 0,
                IndicePaginaNoDisco = indice
            };
            
            ArmazenarNoBuffer(paginaEmMemoria);
            return paginaEmMemoria;
        }

        private bool BufferEstaCheio()
        {
            return buffer.NumeroPaginasNoBuffer == configuaracaoBuffer.LimiteDePaginasEmMemoria;
        }

        private void ArmazenarNoBuffer(PaginaEmMemoria paginaEmMemoria)
        {
            buffer.Armazenar(paginaEmMemoria);
        }

        public void SalvarPagina(int indice)
        {
            var pagina = buffer.Obter(indice);
            arquivoMestre.SalvarPagina(indice, pagina);
        }
    }
}