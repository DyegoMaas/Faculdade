using System;
using System.Drawing;

namespace N2_Exercicio02
{
    public class CirculoReferencia
    {
        public PointF[] Pontos { get; private set; }

        public CirculoReferencia(int raio, int numeroPontos)
        {
            Pontos = new PointF[numeroPontos];
            for (var i = 0; i < numeroPontos; i++)
            {
                var theta = 2 * (float)Math.PI * i / numeroPontos;

                var x = raio * (float)Math.Cos(theta);
                var y = raio * (float)Math.Sin(theta);

                Pontos[i] = new PointF(x, y);
            }
        }
    }
}