namespace Exercicio01.EngineGrafica
{
    public sealed class Ponto4D
    {
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }
        public double W { get; private set; }

        /// Cria o ponto (0,0,0,1).
        public Ponto4D()
            : this(0, 0, 0, 1)
        {
        }

        public Ponto4D(double x, double y, double z = 0d, double w = 1d)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Ponto4D InverterSinal(Ponto4D ponto)
        {
            ponto.X *= -1;
            ponto.Y *= -1;
            ponto.Z *= -1;
            return ponto;
        }
    }
}