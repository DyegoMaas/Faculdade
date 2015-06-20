using System;
using System.Collections.Generic;

namespace RedeNeural.Core.Classificacao.Entradas
{
    public class ExtratorRelacaoAngulos
    {
        public IList<int> ExtrairRelacaoAngulos(IList<TrianguloAmostral> triangulosAmostrais)
        {
            var angulos = new List<int>();
            foreach (var trianguloAmostral in triangulosAmostrais)
            {
                angulos.Add(ExtrairRelacaoAngulos(trianguloAmostral));
            }
            return angulos;
        }

        //http://stackoverflow.com/questions/12891516/math-calculation-to-retrieve-angle-between-two-points
        //https://www.grc.nasa.gov/www/k-12/airplane/Images/tablsin.gif
        //https://br.answers.yahoo.com/question/index?qid=20070808135702AAieQL4
        //http://objetoseducacionais2.mec.gov.br/bitstream/handle/mec/10396/geo0304.htm
        public int ExtrairRelacaoAngulos(TrianguloAmostral triangulo)
        {
            ValidarTrianguloAmostral(triangulo);

            var ponto1Ate2 = DistanciaEntre(triangulo.PontoContorno1, triangulo.PontoContorno2);
            var centroAte1 = DistanciaEntre(triangulo.Centro, triangulo.PontoContorno1);
            var centroAte2 = DistanciaEntre(triangulo.Centro, triangulo.PontoContorno2);
            
            var perimetro = (ponto1Ate2 + centroAte1 + centroAte2) / 2;
            var areaTriangulo = Heron(perimetro, ponto1Ate2, centroAte1, centroAte2);
            var angulo = EncontrarAngulo(areaTriangulo, ponto1Ate2, centroAte1);

            return (int)angulo;

            //var xDiff = triangulo.PontoContorno2.X - triangulo.PontoContorno1.X;
            //var yDiff = triangulo.PontoContorno2.Y - triangulo.PontoContorno1.Y;

            //var anguloX = Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI;

            //if (anguloX < 0)
            //    anguloX += 360;

            //return Math.Abs((int)anguloX);    
        }

        private double EncontrarAngulo(double areaTriangulo, double centroAte1, double ponto1Ate2)
        {
            //area = a * b * sen
            var seno = areaTriangulo / centroAte1 * ponto1Ate2;
            return Math.Asin(seno) * 180 / Math.PI;
        }

        private double Heron(double perimetro, double ladoA, double ladoB, double ladoC)
        {
            return Math.Sqrt(perimetro * (perimetro - ladoA) * (perimetro - ladoB) * (perimetro - ladoC));
        }

        private void ValidarTrianguloAmostral(TrianguloAmostral triangulo)
        {
            if (triangulo.PontoContorno1.X == triangulo.PontoContorno2.X && triangulo.PontoContorno1.Y == triangulo.PontoContorno2.Y)
                throw new Exception("Não é possível calcular o ângulo entre dois pontos iguais");
        }

        private double DistanciaEntre(Vector2 pontoA, Vector2 pontoB)
        {
            var catetoAdjacente = pontoB.X - pontoA.X;
            var catetoOposto = pontoB.Y - pontoA.Y;

            var hipotenusa = Math.Sqrt(catetoAdjacente * catetoAdjacente + catetoOposto * catetoOposto);
            return hipotenusa;
        }
    }
}