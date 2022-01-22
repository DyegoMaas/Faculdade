using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analisador.Atributos
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class BindAttribute : Attribute
    {
        public readonly char Caractere;
        public readonly string Destino;

        public BindAttribute(char caractere, string destino)
        {
            this.Caractere = caractere;
            this.Destino = destino;
        }
    }
}
