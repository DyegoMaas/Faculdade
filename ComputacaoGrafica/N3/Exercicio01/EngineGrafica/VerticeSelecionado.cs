namespace Exercicio01.EngineGrafica
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
            DonoDoPonto.ExcluirPonto(Ponto);
        }
    }
}