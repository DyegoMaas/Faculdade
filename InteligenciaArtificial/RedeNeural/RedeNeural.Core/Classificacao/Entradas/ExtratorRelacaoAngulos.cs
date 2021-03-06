﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace RedeNeural.Core.Classificacao.Entradas
{
    public class ExtratorRelacaoAngulos
    {
        public IList<int> ExtrairRelacaoAngulos(IList<GrupoAmostralAmostral> triangulosAmostrais)
        {
            return triangulosAmostrais.Select(ExtrairRelacaoAngulos).ToList();
        }

        //http://stackoverflow.com/questions/12891516/math-calculation-to-retrieve-angle-between-two-points
        //https://www.grc.nasa.gov/www/k-12/airplane/Images/tablsin.gif
        //https://br.answers.yahoo.com/question/index?qid=20070808135702AAieQL4
        //http://objetoseducacionais2.mec.gov.br/bitstream/handle/mec/10396/geo0304.htm

        //https://www.khanacademy.org/math/linear-algebra/vectors_and_spaces/dot_cross_products/v/defining-the-angle-between-vectors
        //https://www.mathsisfun.com/algebra/trig-cosine-law.html
        //https://www.mathsisfun.com/algebra/trig-solving-triangles.html
        //http://www.calculatorsoup.com/calculators/geometry-plane/triangle-theorems.php
        //https://www.youtube.com/watch?v=50XUgGKd8pw
        public int ExtrairRelacaoAngulos(GrupoAmostralAmostral grupo)
        {
            ValidarTrianguloAmostral(grupo);

            var a = DistanciaEntre(grupo.PontoContorno2, grupo.PontoContorno3);
            var b = DistanciaEntre(grupo.Centro, grupo.PontoContorno2);
            var c = DistanciaEntre(grupo.Centro, grupo.PontoContorno3);
            var coseno = (-(c * c) + a * a + b * b) / 2 / a / b;
            var angulo23 = Math.Acos(coseno) * 180 / Math.PI;


            var a2 = DistanciaEntre(grupo.PontoContorno1, grupo.PontoContorno2);
            var b2 = b;
            var c2 = DistanciaEntre(grupo.Centro, grupo.PontoContorno1);
            coseno = (-(c2 * c2) + a2 * a2 + b2 * b2) / 2 / a2 / b2;
            var angulo21 = Math.Acos(coseno) * 180 / Math.PI;
            //var uDotV = Grupo.PontoContorno1.X * Grupo.PontoContorno2.X +
            //        Grupo.PontoContorno1.Y * Grupo.PontoContorno2.Y; //P1->P2
            //var u = Math.Sqrt(Grupo.PontoContorno1.X * Grupo.PontoContorno1.X + Grupo.PontoContorno1.Y * Grupo.PontoContorno1.Y);
            //var v = Math.Sqrt(Grupo.PontoContorno2.X * Grupo.PontoContorno2.X + Grupo.PontoContorno2.Y * Grupo.PontoContorno2.Y);
            //var coseno = uDotV / (u * v);

            var angulo = (int)Math.Round(angulo21) + (int)Math.Round(angulo23);
            return angulo; 
        }

        private void ValidarTrianguloAmostral(GrupoAmostralAmostral grupo)
        {
            if (grupo.PontoContorno1.X == grupo.PontoContorno2.X && grupo.PontoContorno1.Y == grupo.PontoContorno2.Y)
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