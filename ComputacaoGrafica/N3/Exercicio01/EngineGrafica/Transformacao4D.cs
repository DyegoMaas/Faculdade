using System;

namespace Exercicio01.EngineGrafica
{
    public class Transformacao4D
    {
        public const double DegToRad = 0.017453292519943295769236907684886;

        /// \brief Cria uma matriz de Trasnformacao com uma matriz Identidade.
        private readonly double[] matriz = {	
                1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1};

        public double[] Data
        {
            get { return matriz; }
            set
            {
                for (var i = 0; i < matriz.Length; i++)
                {
                    matriz[i] = (value[i]);
                }   
            }
        }

        /// Atribui os valores de uma matriz Identidade a matriz de Transformacao.
        public void AtribuirIdentidade()
        {
            for (int i = 0; i < 16; ++i)
            {
                matriz[i] = 0.0;
            }
            matriz[0] = matriz[5] = matriz[10] = matriz[15] = 1.0;
        }

        /// Atribui os valores de Translacao (tx,ty,tz) a matriz de Transformacao.
        /// Elemento Neutro eh 0 (zero).
        public void AtribuirTranslacao(double tx, double ty, double tz)
        {
            AtribuirIdentidade();
            matriz[12] = tx;
            matriz[13] = ty;
            matriz[14] = tz;
        }

        /// Atribui o valor de Escala (Ex,Ey,Ez) a matriz de Transformacao.
        /// Elemento Neutro eh 1 (um).
        /// Se manter os valores iguais de Ex,Ey e Ez o objeto vai ser reduzido ou ampliado proporcionalmente.
        public void AtribuirEscala(double sX, double sY, double sZ)
        {
            AtribuirIdentidade();
            matriz[0] = sX;
            matriz[5] = sY;
            matriz[10] = sZ;
        }

        /// Atribui o valor de Rotacao (angulo) no eixo X a matriz de Transformacao.
        /// Elemento Neutro eh 0 (zero).
        public void AtribuirRotacaoX(double radianos)
        {
            AtribuirIdentidade();
            matriz[5] = Math.Cos(radianos);
            matriz[9] = -Math.Sin(radianos);
            matriz[6] = Math.Sin(radianos);
            matriz[10] = Math.Cos(radianos);
        }

        /// Atribui o valor de Rotacao (angulo) no eixo Y a matriz de Transformacao.
        /// Elemento Neutro eh 0 (zero).
        public void AtribuirRotacaoY(double radianos)
        {
            AtribuirIdentidade();
            matriz[0] = Math.Cos(radianos);
            matriz[8] = Math.Sin(radianos);
            matriz[2] = -Math.Sin(radianos);
            matriz[10] = Math.Cos(radianos);
        }

        /// Atribui o valor de Rotacao (angulo) no eixo Z a matriz de Transformacao.
        /// Elemento Neutro eh 0 (zero).
        public void AtribuirRotacaoZ(double radianos)
        {
            AtribuirIdentidade();
            matriz[0] = Math.Cos(radianos);
            matriz[4] = -Math.Sin(radianos);
            matriz[1] = Math.Sin(radianos);
            matriz[5] = Math.Cos(radianos);
        }

        public Ponto4D TransformarPonto(Ponto4D point)
        {
            var pointResult = new Ponto4D(
                    matriz[0] * point.X + matriz[4] * point.Y + matriz[8] * point.Z + matriz[12] *  point.W,
                    matriz[1] * point.X + matriz[5] * point.Y + matriz[9] * point.Z + matriz[13] *  point.W,
                    matriz[2] * point.X + matriz[6] * point.Y + matriz[10] * point.Z + matriz[14] * point.W,
                    matriz[3] * point.X + matriz[7] * point.Y + matriz[11] * point.Z + matriz[15] * point.W);
            return pointResult;
        }

        public Transformacao4D TransofrmarMatriz(Transformacao4D t)
        {
            var result = new Transformacao4D();
            for (int i = 0; i < 16; ++i)
                result.matriz[i] =
                      matriz[i % 4] * t.matriz[i / 4 * 4] + matriz[(i % 4) + 4] * t.matriz[i / 4 * 4 + 1]
                    + matriz[(i % 4) + 8] * t.matriz[i / 4 * 4 + 2] + matriz[(i % 4) + 12] * t.matriz[i / 4 * 4 + 3];
            return result;
        }

        public double GetElement(int index)
        {
            return matriz[index];
        }

        public void SetElement(int index, double value)
        {
            matriz[index] = value;
        }

        public void ImprimirMatriz() {
            Console.WriteLine("______________________");
            Console.WriteLine("|" + GetElement( 0) + " | "+ GetElement( 4) + " | " + GetElement( 8) + " | "+ GetElement(12));
            Console.WriteLine("|" + GetElement( 1) + " | "+ GetElement( 5) + " | " + GetElement( 9) + " | "+ GetElement(13));
            Console.WriteLine("|" + GetElement( 2) + " | "+ GetElement( 6) + " | " + GetElement(10) + " | "+ GetElement(14));
            Console.WriteLine("|" + GetElement( 3) + " | "+ GetElement( 7) + " | " + GetElement(11) + " | "+ GetElement(15));
        }
    }
}