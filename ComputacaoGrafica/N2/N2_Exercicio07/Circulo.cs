using System;
using OpenTK;

namespace N2_Exercicio07
{
    public class Circulo
    {
        public Vector2 Centro;
        public float Raio { get; private set; }

        public Circulo(Vector2 centro, float raio)
        {
            Raio = raio;
            Centro = centro;
        }

        public Vector2 ObterPonto(float ponto, int numeroPontos)
        {
            var theta = 2 * (float)Math.PI * ponto / numeroPontos;
            var x = Centro.X + Raio * (float)Math.Cos(theta);
            var y = Centro.Y + Raio * (float)Math.Sin(theta);

            return new Vector2(x, y);
        }

        public bool EstahDentro(float x, float y)
        {
            var distancia = Distancia(x, y, Centro.X, Centro.Y);
            return distancia <= Raio;
        }

        public Circulo Deslocar(float x, float y)
        {
            return new Circulo(Centro + new Vector2(x, y), Raio);
        }

        private float Distancia(float x1, float y1, float x2, float y2)
        {
            return (float) Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }
    }
}