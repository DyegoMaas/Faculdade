﻿using SimuladorSGBD.Core.GerenciamentoBuffer.Buffer;
using SimuladorSGBD.Core.GerenciamentoBuffer.Paginas;
using System.Collections.Generic;

namespace SimuladorSGBD.Core.GerenciamentoBuffer
{
    public interface IGerenciadorBuffer
    {
        IQuadro ObterPagina(int indice);
        void SalvarPagina(int indice);
        void AtualizarPagina(int indice, char[] conteudo);
        IEnumerable<IResumoPagina> ListarPaginas();
        void Registrar(IBufferChangeObserver observer);
    }
}