using RedeNeural.Core.Classificacao.Saidas;

namespace RedeNeural.Core.Classificacao
{
    public class ResultadoIdeal
    {
        public int[] Bits { get; private set; }
        public ClasseGeometrica Classe { get; private set; }

        public ResultadoIdeal(int[] bits, ClasseGeometrica classe)
        {
            Bits = bits;
            Classe = classe;
        }
    }
}