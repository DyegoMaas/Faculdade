using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analisador.Linguagens.Excecoes
{
    [Serializable]
    public class NoStartException : Exception
    {
        public NoStartException() { }
        public NoStartException(string message) : base(message) { }
        public NoStartException(string message, Exception inner) : base(message, inner) { }
        protected NoStartException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
