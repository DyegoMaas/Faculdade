using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedeNeural.Core.Classificacao.Entradas
{
    public class GeradorValoresRede
    {
        public int[] GerarValor(IList<int> classificacoes)
        {
            var soma = classificacoes.Sum();
            string s = Convert.ToString(soma, 2); //Convert to binary in a string

            int[] bits= s.PadLeft(8, '0') // Add 0's from left
                 .Select(c => int.Parse(c.ToString())) // convert each char to int
                 .Reverse()
                 .Take(7)                 
                 .ToArray(); // Convert IEnumerable from select to Array

            return bits;
        }
    }
}
