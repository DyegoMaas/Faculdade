namespace RedeNeural.Classificacao
{
    public class ParAmostral
    {
        public Vector2 PontoContorno1 { get; private set; }
        public Vector2 PontoContorno2 { get; private set; }

        public ParAmostral(Vector2 pontoContorno1, Vector2 pontoContorno2)
        {
            PontoContorno1 = pontoContorno1;
            PontoContorno2 = pontoContorno2;
        }
    }
}