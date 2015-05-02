using OpenTK;

namespace Exercicio01.EngineGrafica
{
    public class BBox
    {
        public double MinX { get; private set; }
        public double MaxX { get; private set; }
        public double MinY { get; private set; }
        public double MaxY { get; private set; }

        public Vector2d Centro
        {
            get
            {
                return new Vector2d(
                    MinX + ((MaxX - MinX) / 2f),
                    MinY + ((MaxY - MinY) / 2f));
            }
        }

        public BBox(Ponto4D vertice)
        {
            MinX = vertice.X;
            MaxX = vertice.X;
            MinY = vertice.Y;
            MaxY = vertice.Y;
        }

        public void AtualizarCom(Ponto4D novoVertice)
        {
            if (novoVertice.X < MinX) MinX = novoVertice.X;
            if (novoVertice.X > MaxX) MaxX = novoVertice.X;
            if (novoVertice.Y < MinY) MinY = novoVertice.Y;
            if (novoVertice.Y > MaxY) MaxY = novoVertice.Y;
        }
    }
}