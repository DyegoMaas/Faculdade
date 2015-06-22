﻿namespace RedeNeural.Core.Classificacao.Entradas
{
    public class TrianguloAmostral
    {
        private const int NumeroPontosNaBorda = 40;
        public const double DiferencaAngular = 360d / NumeroPontosNaBorda;

        public Vector2 Centro { get; private set; }
        public Vector2 PontoContorno1 { get; private set; }
        public Vector2 PontoContorno2 { get; private set; }
        public Vector2 PontoContorno3 { get; private set; }

        public TrianguloAmostral(Vector2 centro, Vector2 pontoContorno1, Vector2 pontoContorno2, Vector2 pontoContorno3)
        {
            Centro = centro;
            PontoContorno1 = pontoContorno1;
            PontoContorno2 = pontoContorno2;
            PontoContorno3 = pontoContorno3;
        }
    }
}