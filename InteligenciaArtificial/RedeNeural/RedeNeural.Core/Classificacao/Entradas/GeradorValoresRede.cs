using System;
using System.Collections.Generic;
using System.Linq;

namespace RedeNeural.Core.Classificacao.Entradas
{
    public class GeradorValoresRede
    {
        public const int NumeroBits = 7;
        public int[] GerarValor(IList<int> classificacoes)
        {
            var soma = classificacoes.Sum();
            var stringBinaria = Convert.ToString(soma, 2);
            var bits = stringBinaria.PadLeft(NumeroBits, '0') 
                 .Select(c => int.Parse(c.ToString())) 
                    //.OrderBy(c => c) //Colocando os 1s primeiro
                 .Reverse()
                 .Take(NumeroBits)                 
                 .ToArray();

            return bits;
        }
    }
}
