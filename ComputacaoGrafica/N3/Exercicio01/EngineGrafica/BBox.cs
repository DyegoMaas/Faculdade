using System.Collections.Generic;

namespace Exercicio01.EngineGrafica
{
    public class BBox
    {
        public double MinX { get; private set; }
        public double MaxX { get; private set; }
        public double MinY { get; private set; }
        public double MaxY { get; private set; }

        public Ponto4D Centro
        {
            get
            {
                return new Ponto4D(
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

        public void AtualizarCom(Ponto4D vertice)
        {
            if (vertice.X < MinX) MinX = vertice.X;
            if (vertice.X > MaxX) MaxX = vertice.X;
            if (vertice.Y < MinY) MinY = vertice.Y;
            if (vertice.Y > MaxY) MaxY = vertice.Y;
        }

        public void RecalcularPara(IEnumerable<Ponto4D> vertices)
        {
            MinX = MinY = double.MaxValue;
            MaxX = MaxY = double.MinValue;
            foreach (var vertice in vertices)
            {
                if (vertice.X < MinX) MinX = vertice.X;
                if (vertice.X > MaxX) MaxX = vertice.X;
                if (vertice.Y < MinY) MinY = vertice.Y;
                if (vertice.Y > MaxY) MaxY = vertice.Y;    
            }
        }

        public bool Contem(Ponto4D ponto, double tolerancia = 0d)
        {
            return ponto.X >= MinX - tolerancia && ponto.X <= MaxX + tolerancia &&
                   ponto.Y >= MinY - tolerancia && ponto.Y <= MinY + tolerancia;
        }
    }
}