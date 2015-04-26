using System;
using SimuladorSGBD.Core.GerenciamentoBuffer.Buffer;
using SimuladorSGBD.Core.GerenciamentoBuffer.Buffer.LogicaSubstituicao;
using SimuladorSGBD.Core.GerenciamentoBuffer.Buffer.LogicaSubstituicao.PinCount;
using SimuladorSGBD.Core.GerenciamentoBuffer.Paginas;
using SimuladorSGBD.Core.IO;
using System.Collections.Generic;

namespace SimuladorSGBD.Core.GerenciamentoBuffer
{
    public class GerenciadorBuffer : IGerenciadorBuffer, IBufferChangeSubject
    {
        private readonly ILogicaSubstituicaoFactory logicaSubstituicao;
        private readonly IGerenciadorEspacoEmDisco gerenciadorEspacoEmDisco;
        private readonly IPoolDeBuffers buffer;
        private readonly IConfiguracaoBuffer configuracaoBuffer;
        private readonly List<IPinCountChangeObserver> pinCountObservers = new List<IPinCountChangeObserver>();
        private readonly List<IBufferChangeObserver> bufferChangeObservers = new List<IBufferChangeObserver>();

        public GerenciadorBuffer(IGerenciadorEspacoEmDisco gerenciadorEspacoEmDisco, ILogicaSubstituicaoFactory logicaSubstituicao, 
            IPoolDeBuffers buffer, IConfiguracaoBuffer configuracaoBuffer)
        {
            this.logicaSubstituicao = logicaSubstituicao;
            this.gerenciadorEspacoEmDisco = gerenciadorEspacoEmDisco;
            this.buffer = buffer;
            this.configuracaoBuffer = configuracaoBuffer;

            pinCountObservers.Add(logicaSubstituicao.LRU());
            pinCountObservers.Add(logicaSubstituicao.MRU());
        }
        
        public IQuadro ObterQuadro(int indice)
        {
            var quadroBuffer = buffer.Obter(indice);
            if (quadroBuffer != null)
            {
                IncrementarPinCount(quadroBuffer);
                return quadroBuffer;
            }

            if (BufferEstaCheio())
            {
                var indiceParaSubstituir = logicaSubstituicao.LRU().Selecionar();
                if(indiceParaSubstituir > -1)
                {
                    var quadroParaSubstituir = buffer.Obter(indiceParaSubstituir);
                    IncrementarPinCount(quadroParaSubstituir);
                    if (quadroParaSubstituir.Sujo)
                    {
                        gerenciadorEspacoEmDisco.SalvarPagina(quadroParaSubstituir.IndicePaginaNoDisco,
                            quadroParaSubstituir.Pagina);
                    }
                    buffer.Remover(indiceParaSubstituir);
                }
            }

            var pagina = CarregarPaginaDoDisco(indice);
            var quadro = MontarNovoQuadro(pagina, indice);
            ArmazenarNoBuffer(quadro);
            
            return quadro;
        }

        public void LiberarPagina(int indice, bool paginaFoiAlterada)
        {
            var quadro = buffer.Obter(indice);
            DecrementarPinCount(quadro);

            if (paginaFoiAlterada)
                quadro.Sujo = true;
        }

        [Obsolete]
        public void SalvarPagina(int indice)
        {
            var quadro = buffer.Obter(indice);
            gerenciadorEspacoEmDisco.SalvarPagina(indice, quadro.Pagina);
        }

        public void AtualizarPagina(int indice, byte[] conteudo)
        {
            var quadro = buffer.Obter(indice);
            quadro.Sujo = true;
            quadro.Pagina.Conteudo = conteudo;

            foreach (var observer in bufferChangeObservers)
            {
                observer.NotificarAlteracaoBuffer();
            }
        }

        public IEnumerable<IResumoPagina> ListarPaginas()
        {
            return buffer.ListarQuadros();
        }
        
        public void Registrar(IBufferChangeObserver observer)
        {
            bufferChangeObservers.Add(observer);
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
            pinCountObservers.ForEach(l => l.NotificarNovoQuadroComPinCountZero(quadro.IndicePaginaNoDisco));
        }

        private void IncrementarPinCount(IQuadro quadro)
        {
            quadro.PinCount++;
            pinCountObservers.ForEach(l => l.NotificarIncrementoPinCount(quadro.IndicePaginaNoDisco, quadro.PinCount));
        }

        private void DecrementarPinCount(IQuadro quadro)
        {
            quadro.PinCount--;
            pinCountObservers.ForEach(l => l.NotificarDecrementoPinCount(quadro.IndicePaginaNoDisco, quadro.PinCount));
        }
    }
}