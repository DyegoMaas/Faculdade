using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Analisador.Atributos;

namespace Analisador.Linguagens.LinguagensConcretas
{
    public class G1 : LinguagemRegular
    {
        private char[] caracteresSuportados = { 'a', 'b', 'c' };
        public override char[] CaracteresSuportados
        {
            get { return caracteresSuportados; }
        }

        [Start]
        [End]
        [Bind('a', "e1")]
        [Bind('b', "e2")]
        [Bind('c', "e2")]
        private void e0() { }

        [Bind('a', "e3")]
        [Bind('b', "e1")]
        [Bind('c', "e1")]
        private void e1() { }

        [Bind('a', "e1")]
        private void e2() { }

        [Bind('a', "e4")]
        [Bind('b', "e3")]
        [Bind('c', "e3")]
        private void e3() { }

        [End]
        [Bind('b', "e0")]
        [Bind('b', "e1")]
        private void e4() { }
    }
}
