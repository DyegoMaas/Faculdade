using System;
using System.Collections.Generic;
using System.Linq;

namespace RedeNeural.Core.Classificacao.Entradas
{
    public class ExtratorRelacaoAngulos
    {
        public IList<int> ExtrairRelacaoAngulos(IList<TrianguloAmostral> triangulosAmostrais)
        {
            return triangulosAmostrais.Select(ExtrairRelacaoAngulos).ToList();
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
            //var angulo2 = EncontrarAngulo(areaTriangulo, ponto1Ate2, centroAte2);
            //var angulo3 = EncontrarAngulo(areaTriangulo, centroAte1, centroAte2);

            return (int)Math.Round(angulo); 
        }

        private double EncontrarAngulo(double areaTriangulo, double catetoA, double catetoB)
        {
            var seno = areaTriangulo / (catetoA * catetoB / 2);
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