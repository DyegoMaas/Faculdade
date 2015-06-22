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

            var a = DistanciaEntre(triangulo.PontoContorno2, triangulo.PontoContorno3);
            var b = DistanciaEntre(triangulo.Centro, triangulo.PontoContorno2);
            var c = DistanciaEntre(triangulo.Centro, triangulo.PontoContorno3);
            var coseno = (-(c * c) + a * a + b * b) / 2 / a / b;
            var angulo23 = Math.Acos(coseno) * 180 / Math.PI;


            var a2 = DistanciaEntre(triangulo.PontoContorno1, triangulo.PontoContorno2);
            var b2 = b;
            var c2 = DistanciaEntre(triangulo.Centro, triangulo.PontoContorno1);
            coseno = (-(c2 * c2) + a2 * a2 + b2 * b2) / 2 / a2 / b2;
            var angulo21 = Math.Acos(coseno) * 180 / Math.PI;
            //var uDotV = triangulo.PontoContorno1.X * triangulo.PontoContorno2.X +
            //        triangulo.PontoContorno1.Y * triangulo.PontoContorno2.Y; //P1->P2
            //var u = Math.Sqrt(triangulo.PontoContorno1.X * triangulo.PontoContorno1.X + triangulo.PontoContorno1.Y * triangulo.PontoContorno1.Y);
            //var v = Math.Sqrt(triangulo.PontoContorno2.X * triangulo.PontoContorno2.X + triangulo.PontoContorno2.Y * triangulo.PontoContorno2.Y);
            //var coseno = uDotV / (u * v);

            var angulo = (int)Math.Round(angulo21) + (int)Math.Round(angulo23);
            return angulo; 
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