using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analisador.Linguagens.Atributos
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class WithAttribute : Attribute
    {
        public readonly char Caractere;
        public readonly string Destino;

        public WithAttribute(char caractere, string destino)
        {
            this.Caractere = caractere;
            this.Destino = destino;
        }
    }
}
