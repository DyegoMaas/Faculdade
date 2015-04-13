using System.Collections.Generic;

namespace Exercicio01.EngineGrafica
{
    public class Mundo : NoArvoreObjetosGraficos
    {
        public Camera Camera { get; private set; }
        public List<ObjetoGrafico> ObjetosGraficos { get; private set; }

        public Mundo(Camera camera)
        {
            Camera = camera;
            ObjetosGraficos = new List<ObjetoGrafico>();
        }
    }
}