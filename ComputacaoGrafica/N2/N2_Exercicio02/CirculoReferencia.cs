using OpenTK;
using System;

namespace N2_Exercicio02
{
    public class CirculoReferencia
    {
        public Vector2[] Pontos { get; private set; }

        public CirculoReferencia(int raio, int numeroPontos)
        {
            Pontos = new Vector2[numeroPontos];
            for (var i = 0; i < numeroPontos; i++)
            {
                var theta = 2 * (float)Math.PI * i / numeroPontos;

                var x = raio * (float)Math.Cos(theta);
                var y = raio * (float)Math.Sin(theta);

                Pontos[i] = new Vector2(x, y);
            }
        }
    }
}