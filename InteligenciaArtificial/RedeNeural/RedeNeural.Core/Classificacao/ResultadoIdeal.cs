using RedeNeural.Core.Classificacao.Saidas;
using System;

namespace RedeNeural.Core.Classificacao
{
    public class ResultadoIdeal : IEquatable<ResultadoIdeal>
    {
        public int[] Bits { get; private set; }
        public ClasseGeometrica Classe { get; private set; }

        public ResultadoIdeal(int[] bits, ClasseGeometrica classe)
        {
            Bits = bits;
            Classe = classe;
        }

        public bool Equals(ResultadoIdeal other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            if (Bits.Length != other.Bits.Length) return false;

            for (int i = 0; i < Bits.Length; i++)
            {
                if (Bits[i] != other.Bits[i])
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            var stringBits = string.Join("", Bits);
            return stringBits.GetHashCode();
        }
    }
}