namespace Labirinto.EngineGrafica
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
                var ponto = new Ponto4D(x, y);
                var verticeEncontrado = objetoGrafico.ProcurarVertice(ponto);
                if (verticeEncontrado != null)
                {
                    return verticeEncontrado;
                }
            }

            return null;
        }

        public ObjetoGrafico BuscarObjetoSelecionado(double x, double y)
        {
            ObjetoGrafico objetoResultado = null;
            
            foreach (var objeto in ObjetosGraficos)
            {
                var ponto = new Ponto4D(x, y);
                objetoResultado = objeto.BuscarObjetoSelecionado(ponto);

                if (objetoResultado != null)
                {
                    break;
                }
            }

            return objetoResultado;
        }
    }
}