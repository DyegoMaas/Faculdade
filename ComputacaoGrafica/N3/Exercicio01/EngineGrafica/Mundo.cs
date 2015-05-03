namespace Exercicio01.EngineGrafica
{
    public class Mundo : NoGrafoCena
    {
        public Camera Camera { get; private set; }

        public Mundo(Camera camera)
        {
            Camera = camera;
        }

        public Ponto4D BuscarVerticeSelecionado(double x, double y)
        {
            foreach(var objetoGrafico in ObjetosGraficos)
            {
                return objetoGrafico.ProcurarVertice(x, y);
            }

            return null;
        }

        public void RemoverVerticeSelecionado(Ponto4D vertice)
        {
            foreach (var objetoGrafico in ObjetosGraficos)
            {
                objetoGrafico.RemoverVerticeSelecionado(vertice);
            }
        }
    }
}