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

            var a = DistanciaEntre(triangulo.PontoContorno1, triangulo.PontoContorno2);
            var b = DistanciaEntre(triangulo.Centro, triangulo.PontoContorno1);
            var c = DistanciaEntre(triangulo.Centro, triangulo.PontoContorno2);
            var coseno = (-(c * c) + a * a + b * b) / 2 / a / b;
            var anguloC = Math.Acos(coseno) * 180 / Math.PI;

            return (int)Math.Round(anguloC); 
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