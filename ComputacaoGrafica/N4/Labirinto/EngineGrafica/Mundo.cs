namespace Labirinto.EngineGrafica
{
    public class Mundo : NoGrafoCena
    {
        public Camera Camera { get; private set; }

        public Mundo(Camera camera)
        {
            Camera = camera;
        }
    }
}