namespace Labirinto.EngineGrafica
{
    public class VerticeSelecionado
    {
        public Ponto4D Ponto { get; private set; }
        public ObjetoGrafico DonoDoPonto { get; private set; }

        public VerticeSelecionado(Ponto4D ponto, ObjetoGrafico donoDoPonto)
        {
            Ponto = ponto;
            DonoDoPonto = donoDoPonto;
        }

        public void ExcluirDoObjetoGrafico()
        {
            DonoDoPonto.ExcluirVertice(Ponto);
        }

        public void Relocar(double novoX, double novoY)
        {
            Ponto.X = novoX;
            Ponto.Y = novoY;
            DonoDoPonto.RecalcularBBox();
        }
    }
}