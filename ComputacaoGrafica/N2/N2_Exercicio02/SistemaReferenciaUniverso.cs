using System;
using System.Drawing;

namespace N2_Exercicio02
{
    public class SistemaReferenciaUniverso
    {
        public PointF[] PontosCirculo { get; private set; }

        public SistemaReferenciaUniverso()
        {
            const int numeroPontos = 72;
            const int raio = 50;

            PontosCirculo = new PointF[numeroPontos];
            for (var i = 0; i < numeroPontos; i++)
            {
                var theta = 2 * (float)Math.PI * i / numeroPontos;

                var x = raio * (float)Math.Cos(theta);
                var y = raio * (float)Math.Sin(theta);

                PontosCirculo[i] = new PointF(x, y);
            }
        }
    }
}