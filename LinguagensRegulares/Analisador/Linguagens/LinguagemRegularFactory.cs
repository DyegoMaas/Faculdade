using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Analisador.Linguagens.LinguagensConcretas;

namespace Analisador.Linguagens
{
    public static class LinguagemRegularFactory
    {
        public static LinguagemRegular GetLinguagem(LinguagensSuportadas linguagem)
        {
            switch (linguagem)
            {
                case LinguagensSuportadas.G1:
                    return new G1();
                case LinguagensSuportadas.G2:
                    return new G2();
                default:
                    throw new Exception("Linguagem não suportada.");
            }
        }
    }
}
