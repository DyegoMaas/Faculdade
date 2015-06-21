using System;
using System.Collections.Generic;
using System.Linq;

namespace RedeNeural.Core.Classificacao.Entradas
{
    public class GeradorValoresRede
    {
        public int[] GerarValor(IList<int> classificacoes)
        {
            var soma = classificacoes.Sum();
            var stringBinaria = Convert.ToString(soma, 2); 
            var bits= stringBinaria.PadLeft(7, '0') 
                 .Select(c => int.Parse(c.ToString())) 
                 .Reverse()
                 .Take(7)                 
                 .ToArray();

            return bits;
        }
    }
}
