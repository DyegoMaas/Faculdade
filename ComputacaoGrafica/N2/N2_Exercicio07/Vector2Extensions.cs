using System;
using OpenTK;

namespace N2_Exercicio07
{
    public static class Vector2Extensions
    {
        public static bool EstahDentro(this Vector2 ponto, Circulo circulo)
        {
            var distancia = Distancia(ponto.X, ponto.Y, circulo.Centro.X, circulo.Centro.Y);
            return distancia < circulo.Raio; 
        }

        public static Vector2 Deslocar(this Vector2 ponto, float x, float y)
        {
            return new Vector2(ponto.X + x, ponto.Y + y);
        }

        private static float Distancia(float x1, float y1, float x2, float y2)
        {
            return (float)Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }
    }
}