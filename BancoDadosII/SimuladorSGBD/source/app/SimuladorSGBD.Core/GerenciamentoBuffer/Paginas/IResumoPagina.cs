﻿namespace SimuladorSGBD.Core.GerenciamentoBuffer.Paginas
{
    public interface IResumoPagina
    {
        char[] Conteudo { get; }
        int IndiceNoDisco { get; }
        int PinCount { get; }
        bool Sujo { get; }
    }
}