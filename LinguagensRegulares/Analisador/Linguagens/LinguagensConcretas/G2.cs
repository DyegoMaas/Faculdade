using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Analisador.Atributos;
using Analisador.Linguagens;

namespace Analisador.Linguagens.LinguagensConcretas
{
    public class G2 : LinguagemRegular
    {
        private char[] caracteresSuportados = new char[] {'a', 'b', 'c'};
        public override char[] CaracteresSuportados
        {
            get 
            {
                return caracteresSuportados;
            }
        }

        [Start]
        [Bind('a', "e1e2e5e10")]
        [Bind('b', "e1e12")]
        [Bind('c', "e14")]
        private void e0()
        { }

        [Bind('a', "e1e2e3e6e11")]
        [Bind('b', "e1e7")]
        private void e1e2e5e10()
        { }

        [End]
        [Bind('a', "e1e2e3")]
        [Bind('b', "e1")]
        private void e1e2()
        { }

        [End]
        [Bind('c', "e14")]
        private void e14()
        { }

        [End]
        [Bind('a', "e1e2e3e4e5e10")]
        [Bind('b', "e1e4e12")]
        [Bind('c', "e14")]
        private void e1e2e3e6e11()
        { }

        [Bind('a', "e1e2")]
        [Bind('b', "e1e8")]
        [Bind('c', "e9")]
        private void e1e7()
        { }

        [End]
        [Bind('a', "e1e2e3e4")]
        [Bind('b', "e1e4")]
        private void e1e2e3()
        { }

        [Bind('a', "e1e2")]
        [Bind('b', "e1")]
        private void e1()
        { }

        [End]
        [Bind('a', "e1e2e3e4e6e11")]
        [Bind('b', "e1e4e7")]
        private void e1e2e3e4e5e10()
        { }

        [End]
        [Bind('a', "e1e2e4")]
        [Bind('b', "e1e4e13")]
        private void e1e4e12()
        { }

        [Bind('a', "e1e2")]
        [Bind('b', "e1e7")]
        private void e1e8()
        { }

        [End]
        [Bind('c', "e9")]
        private void e9()
        { }

        [End]
        [Bind('a', "e1e2e3e4")]
        [Bind('b', "e1e4")]
        private void e1e2e3e4()
        { }

        [End]
        [Bind('a', "e1e2e4")]
        [Bind('b', "e1e4")]
        private void e1e4()
        { }

        [End]
        [Bind('a', "e1e2e3e4e5e10")]
        [Bind('b', "e1e4e12")]
        [Bind('c', "e14")]
        private void e1e2e3e4e6e11()
        { }

        [End]
        [Bind('a', "e1e2e4")]
        [Bind('b', "e1e4e8")]
        [Bind('c', "e9")]
        private void e1e4e7()
        { }

        [End]
        [Bind('a', "e1e2e3e4")]
        [Bind('b', "e1e4")]
        private void e1e2e4()
        { }

        [Bind('a', "e1e2")]
        [Bind('b', "e1e12")]
        [Bind('c', "e14")]
        private void e1e13()
        { }

        [Bind('a', "e1e2")]
        [Bind('b', "e1e13")]
        private void e1e12()
        { }

        [End]
        [Bind('a', "e1e2e4")]
        [Bind('b', "e1e4e7")]
        private void e1e4e8()
        { }

        [End]
        [Bind('a', "e1e2e4")]
        [Bind('b', "e1e4e12")]
        [Bind('c', "e14")]
        private void e1e4e13()
        { }
    }
}
