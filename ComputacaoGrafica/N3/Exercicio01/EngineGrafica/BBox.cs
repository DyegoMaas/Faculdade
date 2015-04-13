using System;

namespace Exercicio01.EngineGrafica
{
    public class BBox
    {
        public int MinX { get; private set; }
        public int MaxX { get; private set; }
        public int MinY { get; private set; }
        public int MaxY { get; private set; }

        public static BBox Calcular(ObjetoGrafico objeto)
        {
            throw new NotImplementedException();
        }
    }
}