namespace RedeNeural.Core.Classificacao.Entradas
{
    public class TrianguloAmostral
    {
        private const int NumeroPontosNaBorda = 32;
        public const double DiferencaAngular = 360d / NumeroPontosNaBorda;

        public Vector2 Centro { get; private set; }
        public Vector2 PontoContorno1 { get; private set; }
        public Vector2 PontoContorno2 { get; private set; }

        public TrianguloAmostral(Vector2 centro, Vector2 pontoContorno1, Vector2 pontoContorno2)
        {
            Centro = centro;
            PontoContorno1 = pontoContorno1;
            PontoContorno2 = pontoContorno2;
        }
    }
}