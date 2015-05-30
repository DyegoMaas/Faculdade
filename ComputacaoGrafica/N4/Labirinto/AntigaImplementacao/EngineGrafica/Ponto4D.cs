namespace JogoLabirinto.AntigaImplementacao.EngineGrafica
{
    public sealed class Ponto4D
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double W { get; set; }

        private static readonly Ponto4D zero = new Ponto4D();
        public static Ponto4D Zero { get { return zero; } }

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

        public bool EstaProximo(Ponto4D ponto, double tolerancia)
        {
            return ponto.X >= X - tolerancia && ponto.X <= X + tolerancia &&
                   ponto.Y >= Y - tolerancia && ponto.Y <= Y + tolerancia;
        }
    }

    public static class Ponto4DExtensions
    {
        public static Ponto4D InverterSinal(this Ponto4D ponto)
        {
            ponto.X *= -1;
            ponto.Y *= -1;
            ponto.Z *= -1;
            return ponto;
        }
    }
}