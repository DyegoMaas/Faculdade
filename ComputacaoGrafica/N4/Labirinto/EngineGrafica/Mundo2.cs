namespace Labirinto.EngineGrafica
{
    public class Mundo2 : NoGrafoCenaSimples
    {
        public Camera Camera { get; private set; }

        public Mundo2(Camera camera)
        {
            Camera = camera;
        }
    }
}