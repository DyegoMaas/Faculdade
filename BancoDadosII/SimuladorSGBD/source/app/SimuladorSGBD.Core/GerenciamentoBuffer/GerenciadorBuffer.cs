using SimuladorSGBD.Core.IO;

namespace SimuladorSGBD.Core.GerenciamentoBuffer
{
    public class GerenciadorBuffer
    {
        private readonly IArquivoMestre arquivoMestre;

        public GerenciadorBuffer(IArquivoMestre arquivoMestre)
        {
            this.arquivoMestre = arquivoMestre;
        }

        public IPaginaEmMemoria CarregarPagina(int indice)
        {
            return new PaginaEmMemoria
            {
                Dados = arquivoMestre.CarregarPagina(indice).Dados,
                PinCount = 0,
                Sujo = false,
                UltimoAcesso = 0,
                IndicePagina = indice
            };
        }
        
        public void SalvarPagina(int indice, IPaginaEmMemoria pagina)
        {
            throw new System.NotImplementedException();
        }
    }
}