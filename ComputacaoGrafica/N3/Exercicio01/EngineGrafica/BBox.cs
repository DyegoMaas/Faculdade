namespace Exercicio01.EngineGrafica
{
    public class BBox
    {
        public double MinX { get; private set; }
        public double MaxX { get; private set; }
        public double MinY { get; private set; }
        public double MaxY { get; private set; }

        public static BBox Calcular(ObjetoGrafico objeto)
        {
            var bBox = new BBox();
            foreach (var vertice in objeto.Vertices)
            {
                if (vertice.X < bBox.MinX) bBox.MinX = vertice.X;
                if (vertice.X > bBox.MaxX) bBox.MaxX = vertice.X;
                if (vertice.Y < bBox.MinY) bBox.MinY = vertice.X;
                if (vertice.Y < bBox.MaxY) bBox.MaxY = vertice.X;
            }
            return bBox;
        }

        public void AtualizarCom(Ponto4D novoVertice)
        {
            if (novoVertice.X < MinX) MinX = novoVertice.X;
            if (novoVertice.X > MaxX) MaxX = novoVertice.X;
            if (novoVertice.Y < MinY) MinY = novoVertice.X;
            if (novoVertice.Y < MaxY) MaxY = novoVertice.X;
        }
    }
}