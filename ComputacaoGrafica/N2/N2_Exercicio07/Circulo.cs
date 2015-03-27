using System;
using OpenTK;

namespace N2_Exercicio07
{
    public class Circulo
    {
        public Vector2[] Pontos { get; private set; }
        public Vector2 Centro { get; private set; }
        public float Raio { get; private set; }

        public Circulo(Vector2 centro, float raio, int numeroPontos)
        {
            Raio = raio;
            Centro = centro;
            GerarPontos(centro, numeroPontos);
        }

        private void GerarPontos(Vector2 centro, int numeroPontos)
        {
            Pontos = new Vector2[numeroPontos];

            for (var i = 0; i < numeroPontos; i++)
            {
                var theta = 2*(float) Math.PI*i/numeroPontos;
                var x = centro.X + Raio*(float) Math.Cos(theta);
                var y = centro.Y + Raio*(float) Math.Sin(theta);

                Pontos[i] = new Vector2(x, y);
            }
        }

        public bool EstahDentro(float x, float y)
        {
            var distancia = Distancia(x, y, Centro.X, Centro.Y);
            return distancia <= Raio;
        }

        public void Deslocar(float x, float y)
        {
            GerarPontos(Centro + new Vector2(x, y), Pontos.Length);
        }

        private float Distancia(float x1, float y1, float x2, float y2)
        {
            return (float) Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }
    }
}