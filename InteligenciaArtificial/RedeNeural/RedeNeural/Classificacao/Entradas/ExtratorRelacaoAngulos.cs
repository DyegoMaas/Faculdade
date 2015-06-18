using System;
using System.Collections.Generic;
using System.Linq;

namespace RedeNeural.Classificacao
{
    public class ExtratorRelacaoAngulos
    {
        public IList<int> ExtrairRelacaoAngulos(IList<ParAmostral> paresAmostrais)
        {
            return paresAmostrais.Select(ExtrairRelacaoAngulos).ToList();
        }

        //http://stackoverflow.com/questions/12891516/math-calculation-to-retrieve-angle-between-two-points
        public int ExtrairRelacaoAngulos(ParAmostral par)
        {
            ValidarParAmostral(par);

            var xDiff = par.PontoContorno2.X - par.PontoContorno1.X;
            var yDiff = par.PontoContorno2.Y - par.PontoContorno1.Y;
            var angulo = Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI;

            //if (angulo < 0)
            //    angulo += 360;

            return Math.Abs((int)angulo);    
        }

        private void ValidarParAmostral(ParAmostral par)
        {
            if (par.PontoContorno1.X == par.PontoContorno2.X && par.PontoContorno1.Y == par.PontoContorno2.Y)
                throw new Exception("Não é possível calcular o ângulo entre dois pontos iguais");
        }
    }
}