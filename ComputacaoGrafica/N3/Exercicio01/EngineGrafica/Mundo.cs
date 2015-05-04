namespace Exercicio01.EngineGrafica
{
    public class Mundo : NoGrafoCena
    {
        public Camera Camera { get; private set; }

        public Mundo(Camera camera)
        {
            Camera = camera;
        }

        public VerticeSelecionado BuscarVerticeSelecionado(double x, double y)
        {
            foreach(var objetoGrafico in ObjetosGraficos)
            {
                var verticeEncontrado = objetoGrafico.ProcurarVertice(x, y);
                if (verticeEncontrado != null)
                {
                    return verticeEncontrado;
                }
            }

            return null;
        }
    }
}