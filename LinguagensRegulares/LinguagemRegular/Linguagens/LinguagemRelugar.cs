using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Analisador.Linguagens.Atributos;

namespace Analisador.Linguagens
{
    public abstract class LinguagemRegular
    {
        public abstract char[] CaracteresSuportados { get; }

        private LinguagemRegular() { }

        public static LinguagemRegular GetLinguagem(LinguagensSuportadas linguagem)
        {
            switch (linguagem)
            {
                case LinguagensSuportadas.G1:
                    return new G1();
                default:
                    throw new Exception("Linguagem não suportada");
            }
        }

        private class G1 : LinguagemRegular
        {
            private char[] caracteresSuportados = { 'a', 'b', 'c' };
            public override char[] CaracteresSuportados
            {
                get { return caracteresSuportados; }
            }

            [Start]
            [With('a', "R")]
            [With('a', "")]
            [End]
            private void S() { }

            [With('b', "S")]
            [End]
            private void R() { }

        }
    }
}
