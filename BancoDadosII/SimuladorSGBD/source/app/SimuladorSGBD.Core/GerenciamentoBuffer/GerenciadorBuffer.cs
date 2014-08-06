using SimuladorSGBD.Core.GerenciamentoBuffer.Buffer;
using SimuladorSGBD.Core.GerenciamentoBuffer.Paginas;
using SimuladorSGBD.Core.IO;
using System.Collections.Generic;

namespace SimuladorSGBD.Core.GerenciamentoBuffer
{
    public class GerenciadorBuffer : IGerenciadorBuffer
    {
        private readonly ILogicaSubstituicao logicaSubstituicao;
        private readonly IGerenciadorEspacoEmDisco gerenciadorEspacoEmDisco;
        private readonly IPoolDeBuffers buffer;
        private readonly IConfiguracaoBuffer configuracaoBuffer;
        
        public GerenciadorBuffer(IGerenciadorEspacoEmDisco gerenciadorEspacoEmDisco, ILogicaSubstituicao logicaSubstituicao, 
            IPoolDeBuffers buffer, IConfiguracaoBuffer configuracaoBuffer)
        {
            this.logicaSubstituicao = logicaSubstituicao;
            this.gerenciadorEspacoEmDisco = gerenciadorEspacoEmDisco;
            this.buffer = buffer;
            this.configuracaoBuffer = configuracaoBuffer;
        }
        
        public IQuadro ObterPagina(int indice)
        {
            var quadroBuffer = buffer.Obter(indice);
            if (quadroBuffer != null)
            {
                quadroBuffer.PinCount++;
                return quadroBuffer;
            }

            if (BufferEstaCheio())
            {
                var indiceParaSubstituir = logicaSubstituicao.Selecionar();
                var quadroParaSubstituir = buffer.Obter(indiceParaSubstituir);
                quadroParaSubstituir.PinCount++;
                if (quadroParaSubstituir.Sujo)
                {
                    gerenciadorEspacoEmDisco.SalvarPagina(quadroParaSubstituir.IndicePaginaNoDisco,
                        quadroParaSubstituir.Pagina);
                }
                buffer.Remover(indiceParaSubstituir);
            }

            var pagina = CarregarPaginaDoDisco(indice);
            var quadro = MontarNovoQuadro(pagina, indice);
            ArmazenarNoBuffer(quadro);

            return quadro;
        }

        public void LiberarPagina(int indice, bool paginaFoiAlterada)
        {
            var quadro = buffer.Obter(indice);
            quadro.PinCount--;

            if (paginaFoiAlterada)
                quadro.Sujo = true;
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
            return buffer.ListarQuadros();
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