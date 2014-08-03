using System;
using System.Collections.Generic;
using SimuladorSGBD.Core.IO;

namespace SimuladorSGBD.Core.GerenciamentoBuffer
{
    public class GerenciadorBuffer
    {
        private readonly IArquivoMestre arquivoMestre;
        private readonly IConfiguaracaoBuffer configuaracaoBuffer;
        private readonly IDictionary<int, IPaginaEmMemoria> buffer = new Dictionary<int, IPaginaEmMemoria>();

        public GerenciadorBuffer(IArquivoMestre arquivoMestre, IConfiguaracaoBuffer configuaracaoBuffer)
        {
            this.arquivoMestre = arquivoMestre;
            this.configuaracaoBuffer = configuaracaoBuffer;
        }

        public IPaginaEmMemoria CarregarPagina(int indice)
        {
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

        private void ArmazenarNoBuffer(PaginaEmMemoria paginaEmMemoria)
        {
            buffer.Add(paginaEmMemoria.IndicePaginaNoDisco, paginaEmMemoria);
        }

        public void SalvarPagina(int indice)
        {
            arquivoMestre.SalvarPagina(indice, buffer[indice]);
        }
    }
}