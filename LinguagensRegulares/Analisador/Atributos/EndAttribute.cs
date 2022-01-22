using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analisador.Atributos
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class EndAttribute : Attribute
    {
        public EndAttribute() { }
    }
}
